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
                    OleDbCommand oleCmdSelect = new OleDbCommand("SELECT ProductId, Quantity FROM [Sales$B3:C3]", excelConnection);

                    //try
                    //{
                        OleDbDataReader reader = oleCmdSelect.ExecuteReader();
                        while (reader.Read())
                        {
                            Console.WriteLine("Enter");
                            int name = int.Parse(reader["ProductID"].ToString());
                            int score = int.Parse(reader["Quantity"].ToString());
                            Console.WriteLine("{0} with scores {1}", name, score);
                        }

                        reader.Close();
                        
                    //}
                    //catch (Exception)
                    //{
                        
                    //}
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