using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Standard.Licensing;
using Standard.Licensing.Validation;
using System.Data.SqlClient;

namespace MyProject
{
   class LicenseValidation
    {

        public bool printing;
        public bool isLicensed;
        private string publicKey;
        public string apteka;
        public bool silent;
        public LicenseValidation()
        {
            publicKey = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAESTEkmOb/VBct6+M7Ifjp8AGPC/F6ZVhCKESYdm5h1ZzkNh+yOS1c3WbXYBcl2Ht4nM9L3wPmFbLpf9phC2ou2w==";
            printing = false;
            isLicensed = false;
            apteka = "NONE";
            silent = true;

            

            string licenseFile = File.ReadAllText("license.lic");
            var license = License.Load(licenseFile);
            var validationFailures = license.Validate()
                                            .ExpirationDate()
                                            .When(lic => lic.Type == LicenseType.Trial)
                                            .And()
                                            .Signature(publicKey)
                                            .AssertValidLicense();

            apteka = license.Customer.Name;
            string ConnectionString = Properties.Settings.Default.ConnectionString;
            foreach (var failure in validationFailures)
                Console.WriteLine(failure.GetType().Name + ": " + failure.Message + " - " + failure.HowToResolve);


            if (license.ProductFeatures.Get("Silent")=="yes")
            {
                silent = true;
            }
            else
            {
                silent = false;
            }

            if (license.ProductFeatures.Get("Print")=="yes")
            {
                printing = true;
            }
            else
            {
                printing = false;
            }

            string aptekaDB;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = @"select name from CONTRACTOR where ID_CONTRACTOR = DBO.FN_CONST_CONTRACTOR_SELF()";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                   // comm.Parameters.AddWithValue("@id", id);

                    using (var reader = comm.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new Exception("Something is very wrong");

                        //int ordId = reader.GetOrdinal("id");
                        int ordName = reader.GetOrdinal("name");
                        //int ordPath = reader.GetOrdinal("path");

                        //image.Id = reader.GetInt32(ordId);
                        aptekaDB = reader.GetString(ordName);
                        //image.Path = reader.GetString(ordPath);

                        
                    }
                }
            }




            if (validationFailures.Any()|| !aptekaDB.Contains(apteka))
            {
                Console.WriteLine("Licensing error");
                isLicensed = false;
            }
            else
            {
                isLicensed = true;
            }


        }

    }
    
}
