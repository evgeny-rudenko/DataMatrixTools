namespace MyProject
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dtKizBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.KizDataSet = new MyProject.KizDataSet();
            this.Button = new System.Windows.Forms.Button();
            this.Input = new System.Windows.Forms.TextBox();
            this.dtAllCodesKizBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cbAllRows = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dtKizBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KizDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAllCodesKizBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dtKizBindingSource
            // 
            this.dtKizBindingSource.DataMember = "dtKiz";
            this.dtKizBindingSource.DataSource = this.KizDataSet;
            // 
            // KizDataSet
            // 
            this.KizDataSet.DataSetName = "KizDataSet";
            this.KizDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Button
            // 
            this.Button.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.Button.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.Button.Location = new System.Drawing.Point(324, 10);
            this.Button.Name = "Button";
            this.Button.Size = new System.Drawing.Size(133, 47);
            this.Button.TabIndex = 6;
            this.Button.Text = "Сформировать";
            this.Button.Click += new System.EventHandler(this.ButtonClick);
            // 
            // Input
            // 
            this.Input.Location = new System.Drawing.Point(12, 12);
            this.Input.MaxLength = 3116;
            this.Input.Multiline = true;
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(306, 45);
            this.Input.TabIndex = 3;
            this.Input.Text = "1602/ПН-00004871";
            // 
            // rtb
            // 
            this.rtb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb.Location = new System.Drawing.Point(463, 458);
            this.rtb.Name = "rtb";
            this.rtb.Size = new System.Drawing.Size(676, 168);
            this.rtb.TabIndex = 9;
            this.rtb.Text = "";
            // 
            // reportViewer1
            // 
            this.reportViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportViewer1.DocumentMapWidth = 1;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.dtKizBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "MyProject.Report1.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(463, 12);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(676, 440);
            this.reportViewer1.TabIndex = 7;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 90);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(444, 536);
            this.dataGridView1.TabIndex = 12;
            // 
            // cbAllRows
            // 
            this.cbAllRows.AutoSize = true;
            this.cbAllRows.Location = new System.Drawing.Point(16, 64);
            this.cbAllRows.Name = "cbAllRows";
            this.cbAllRows.Size = new System.Drawing.Size(215, 17);
            this.cbAllRows.TabIndex = 13;
            this.cbAllRows.Text = "Весь документ приходная накладная";
            this.cbAllRows.UseVisualStyleBackColor = true;
            this.cbAllRows.CheckedChanged += new System.EventHandler(this.cbAllRows_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(1151, 638);
            this.Controls.Add(this.cbAllRows);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.rtb);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.Button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Печать датаматрикс кодов   APTEKA27.COM";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtKizBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KizDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtAllCodesKizBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Button;
        private System.Windows.Forms.TextBox Input;
        private System.Windows.Forms.BindingSource dtKizBindingSource;
        private KizDataSet KizDataSet;
        //private DataSetF3Tail dsF3Tail;
        private System.Windows.Forms.DataGridViewTextBoxColumn dOCNUMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gOODNAMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn kIZDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bARCODEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gTINDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gTINSGTINDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sGTINDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDLOTGLOBALDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDOCUMENTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn qUANTITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEMAINQTYDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource dtAllCodesKizBindingSource;
        private System.Windows.Forms.RichTextBox rtb;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.CheckBox cbAllRows;
        //private DataSetF3TailTableAdapters.dtAllCodesKizTableAdapter dtAllCodesKizTableAdapter;
    }
}

