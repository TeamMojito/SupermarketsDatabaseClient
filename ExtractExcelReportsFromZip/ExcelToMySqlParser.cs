using System;
using System.Data.OleDb;
using System.IO;

namespace ExtractExcelReportsFromZip
{
    public class ExcelToMySqlParser
    {
        private string reportsDirectory;
        private static readonly string ExcelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES"";";

        public ExcelToMySqlParser(string reportsDirectory)
        {
            this.ReportsDirectory = reportsDirectory;
        }

        public void NemogaDaTiIzmislqImeSq()
        {
            string[] directories = Directory.GetDirectories(this.reportsDirectory);

            foreach (var directory in directories)
            {
                Console.WriteLine(directory);
                ReadExcelReport(directory);
            }
        }
  
        private void ReadExcelReport(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            
            foreach (var file in files)
            { 
                string connectionString = string.Format(ExcelConnectionString, file);
                OleDbConnection excelConnection = new OleDbConnection(connectionString);

                excelConnection.Open();

                using (excelConnection)
                {
                    OleDbCommand oleCmdSelect = new OleDbCommand("SELECT * FROM [Sales$]", excelConnection);
                    
                    OleDbDataReader reader = oleCmdSelect.ExecuteReader();
                    // Skip the table name
                    reader.Read();
                    // Skip the identifiers names
                    reader.Read();

                    while (reader.Read())
                    {
                        if (reader[0].ToString() == "Total sum:")
                        {
                            break;
                        }

                        if (reader[0].ToString() == "…")
                        {
                            continue;
                        }

                        string productId = reader[0].ToString();
                        string quantity = reader[1].ToString();
                        string unitPrice = reader[2].ToString();
                        string sum = reader[3].ToString();

                        Console.WriteLine("Product id: {0}", productId);
                        Console.WriteLine("Quantity {0}", quantity);
                        Console.WriteLine("Unit Price {0}", unitPrice);
                        Console.WriteLine("Sum {0}", sum);
                    }

                    reader.Close();
                }
            }
        }

        public string ReportsDirectory
        {
            get
            {
                return this.reportsDirectory;
            }
            set
            {
                this.reportsDirectory = value;
            }
        }
    }
}