namespace SupermarketsDatabaseClient
{
    using System;
    using System.IO;
    using System.Linq;
    using MSSQLSupermarketEntityFramework;
    using OpenAccessMySqlSupermarketEntityModel;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    class Program
    {
        static void Main()
        {
            //MigrateDataFromMySqlToMSSqlServer();
            //ZipExtractor zipExtractor = new ZipExtractor(@"D:\Trash\Sample-Sales-Reports.zip");
            //var directory = zipExtractor.ExtractContentTo(@"..\..\temp\");
            //ExcelToMySqlParser excelToMySqlParser = new ExcelToMySqlParser(@"..\..\temp\");
            //excelToMySqlParser.ReadFilesFromExcel();
            //zipExtractor.RemoveExtractedDirectory(directory);
            PdfReportert.CreateReport();
            XmlGenerator.GenerateXmlByVendors();
        }

        public static void MigrateDataFromMySqlToMSSqlServer()
        {
            using (var mySqlSuperMarketEntities = new OpenAccessMySqlSupermarketEntityModel())
            {
                using (var msSqlSeverSupermarketEntities = new SupermarketEntities())
                {
                    foreach (var mySqlMeasure in mySqlSuperMarketEntities.Measures)
                    {
                        msSqlSeverSupermarketEntities.Measures.Add(new MSSQLSupermarketEntityFramework.Measure()
                        {
                            MeasureId = mySqlMeasure.MeasureId,
                            MeasureName = mySqlMeasure.MeasureName,
                        });
                    }

                    foreach (var mySqlMeasure in mySqlSuperMarketEntities.Vendors)
                    {
                        msSqlSeverSupermarketEntities.Vendors.Add(new MSSQLSupermarketEntityFramework.Vendor()
                        {
                            VendorId = mySqlMeasure.VendorId,
                            VendorName = mySqlMeasure.VendorName,
                        });
                    }

                    foreach (var mySqlMeasure in mySqlSuperMarketEntities.Products)
                    {
                        msSqlSeverSupermarketEntities.Products.Add(new MSSQLSupermarketEntityFramework.Product()
                        {
                            BasePrice = mySqlMeasure.BasePrice,
                            MeasureId = mySqlMeasure.MeasureId,
                            ProductId = mySqlMeasure.ProductId,
                            ProductName = mySqlMeasure.ProductName,
                            VendorId = mySqlMeasure.VendorId
                        });
                    }

                    msSqlSeverSupermarketEntities.SaveChanges();
                }
            }
        }
    }
}