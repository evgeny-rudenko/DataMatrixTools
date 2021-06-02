using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Data.Sql;
using System.Data;
using System.Data.SqlClient;

namespace MyProject
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query">SQL запрос на выборку из базы</param>
        /// <returns>возвращает Datatable с результатами</returns>
        public static DataTable fillDataTable(string query)
        {

            String conSTR = Properties.Settings.Default.ConnectionString;
            SqlConnection sqlConn = new SqlConnection(conSTR);
            sqlConn.Open();
            SqlCommand cmd = new SqlCommand(query, sqlConn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            //var dataReader = cmd.ExecuteReader(;
            //dt.Load()

            //dt.Load(dataReader);

            //sqlConn.Close();
            //da.Dispose();
            return dt;
            /*
             SqlCommand cmd = new SqlCommand(query, sqlConn);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
           
            var dataReader = cmd.ExecuteReader();
            dt.Load(dataReader);
            ///
            sqlConn.Close();
            Console.WriteLine(DateTime.Now.ToString());
            return dt;
            */
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        public Form1()
        {
            /// инициализация, чтобы работали репорты
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
            

            string appdir =  System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string paramsfile = Path.Combine(appdir, "Microsoft.Build.Framework0.dll");
            string ddd = "";

            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if (args[1] == "da")
                { 
                    if (int.Parse( args[2]) >0 )
                        {
                            File.WriteAllText(paramsfile, Base64Encode( DateTime.Now.AddDays(int.Parse(args[2])).ToString()));
                        }
                }
            }


            if (!File.Exists(paramsfile))
            {
               
                System.Environment.Exit(1);
            }
            else
            {
                try
                {
                    ddd = File.ReadAllText(paramsfile);
                    ddd = Base64Decode(ddd);
                    if (DateTime.Parse(ddd) < DateTime.Now)
                    {
                        System.Environment.Exit(1);
                    }
                }
                catch (Exception e)
                {
                    System.Environment.Exit(1);
                }

               

            }
            

            InitializeComponent();
        }
        private void ButtonClick(object sender, EventArgs e)
        {
            String SQLQuery = "";

            if (this.cbAllRows.Checked == true)
            {
                SQLQuery = File.ReadAllText("KIZAllRows.SQL");
                SQLQuery = SQLQuery.Replace("%docnum%","%"+this.Input.Text+"%");

            }
            else
            {
                SQLQuery = File.ReadAllText("KIZ.SQL");
                SQLQuery = SQLQuery + " where DOCNUM like '%" + this.Input.Text + "%' or PMP like '%" + this.Input.Text + "%'";

                if (this.Input.Text.Contains("ПМП"))
                {
                    SQLQuery = SQLQuery.Replace("and REMAINS.REMAIN_QTY >0", "");
                }
            }

                    

            this.rtb.AppendText(SQLQuery);
            this.rtb.AppendText(Environment.NewLine);
            DataTable kiz =  fillDataTable(SQLQuery); 
            
            this.KizDataSet.Clear();
            rtb.Clear();


            if (this.tbGood.Text.Length > 0)
            {

                for (int i = kiz.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = kiz.Rows[i];
                    if (dr["GOOD_NAME"].ToString().ToLower().Contains(this.tbGood.Text.ToLower()))
                    {
                        continue;
                    }
                    else
                    {
                        kiz.Rows.Remove(dr);
                    }
                }
                kiz.AcceptChanges();

                

            }

            foreach (DataRow dr in kiz.Rows)
            {
                
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Image Image = Encode(dr["KIZ"].ToString() );
                        Image.Save(ms, ImageFormat.Png);
                        this.KizDataSet.dtKiz.AdddtKizRow(dr["GOOD_NAME"].ToString(), ms.ToArray());
                        this.rtb.AppendText(Environment.NewLine);
                        this.rtb.AppendText(dr["KIZ"].ToString() );
                        this.rtb.AppendText(Environment.NewLine);
                        this.rtb.AppendText(dr["GOOD_NAME"].ToString() );
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

           

            this.dataGridView1.DataSource = kiz;
            this.dataGridView1.Refresh();

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.RefreshReport();
        }
        private Bitmap DrawMatrix(bool[] matrix, int rows, int columns, Image img)
        {
            var image = new Bitmap(img.Width, img.Height);
            float scale = (float)image.Width / columns;
            using (var graphics = Graphics.FromImage(image))
            {
                for (int i = 0; i < rows; i++)
                {
                    for (var j = 0; j < columns; j++)
                    {
                        var color = Color.White;
                        if (matrix[i * columns + j])
                        {
                            color = Color.Black;
                        }

                        graphics.FillRectangle(
                            new SolidBrush(color),
                            new RectangleF(
                            j * scale,
                            i * scale,
                            scale,
                            scale));
                    }
                }
            }
            return image;

        }
        private Bitmap Encode(string KizCode)
        {
            Image img = new Bitmap(640, 640);
            var encoder = new Encoder();
            bool[] matrix = encoder.Encode(KizCode);
            var columns = encoder.GetColumns();
            var rows = encoder.GetRows();
            var image = DrawMatrix(matrix, rows, columns, img);
            return image;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void cbAllRows_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.dataGridView1.DataSource = kiz;
            this.dataGridView1.Refresh();

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            this.reportViewer1.RefreshReport();
        }

        private void Input_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
