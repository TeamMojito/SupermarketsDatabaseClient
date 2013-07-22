namespace ExtractExcelReportsFromZip
{
    using System;
    using System.Data.OleDb;
    using System.IO;
    using MSSQLSupermarketEntityFramework;

    public class ExcelToMySqlParser
    {
        private string reportsDirectory;
        private static readonly string ExcelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES"";";

        public ExcelToMySqlParser(string reportsDirectory)
        {
            this.ReportsDirectory = reportsDirectory;
        }

        public void ReadFilesFromExcel()
        {
            string[] directories = Directory.GetDirectories(this.reportsDirectory);

            foreach (var directory in directories)
            {
                ReadExcelReport(directory);
            }
        }
  
        private void ReadExcelReport(string directory)
        {
            string[] files = Directory.GetFiles(directory);
            
            foreach (var file in files)
            { 
                OleDbConnection excelConnection = new OleDbConnection(string.Format(ExcelConnectionString, file));

                excelConnection.Open();

                using (excelConnection)
                {
                    OleDbCommand oleCmdSelect = new OleDbCommand("SELECT * FROM [Sales$]", excelConnection);
                    
                    OleDbDataReader reader = oleCmdSelect.ExecuteReader();
                    
                    // Skip the table name
                    reader.Read();
                    string [] tokens = directory.Split('\\');
                    DateTime reportDate = DateTime.Parse(tokens[tokens.Length - 1]);
                    string supermarketName = reader[0].ToString();
                    // Skip the identifiers names
                    reader.Read();

                    while (reader.Read())
                    {
                        if (reader[0].ToString() == "…")
                        {
                            break;
                        }

                        int productId = int.Parse(reader[0].ToString());
                        int quantity = int.Parse(reader[1].ToString());
                        decimal unitPrice = decimal.Parse(reader[2].ToString());
                        decimal sum = decimal.Parse(reader[3].ToString());

                        AddRecordToDatabase(productId, quantity, unitPrice, sum, reportDate, supermarketName);
                    }

                    reader.Close();
                }
            }
        }

        private void AddRecordToDatabase(int productId, int quantity, decimal unitPrice, decimal sum, DateTime reportDate, string supermarketName)
        {
            using (var supermarketEntities = new SupermarketEntities())
            {
                var report = new Report()
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    Sum = sum,
                    ReportDate = reportDate,
                    Supermarket = supermarketName
                };

                supermarketEntities.Reports.Add(report);
                supermarketEntities.SaveChanges();
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