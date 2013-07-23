namespace SupermarketsDatabaseClient
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using ExtractExcelReportsFromZip;
    using MSSQLSupermarketEntityFramework;
    using OpenAccessMySqlSupermarketEntityModel;

    class Program
    {
        static void Main()
        {
            //Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            //MigrateDataFromMySqlToMSSqlServer();
            //ZipExtractor zipExtractor = new ZipExtractor(@"D:\Trash\Sample-Sales-Reports.zip");
            //var directory = zipExtractor.ExtractContentTo(@"..\..\temp\");
            //ExcelToMySqlParser excelToMySqlParser = new ExcelToMySqlParser(@"..\..\temp\");
            //excelToMySqlParser.ReadFilesFromExcel();
            //zipExtractor.RemoveExtractedDirectory(directory);
            //PdfReportert.CreateReport();
            //XmlGenerator.GenerateXmlByVendors();
            //MongoDBConnector.CreateProductsReportData("mongodb://localhost/");
            //XmlToMongoDbAndSqlServer.SaveToMongoDb("mongodb://localhost/", @"D:\Projects\TeamMojito\SupermarketsDatabaseClient\Vendors-Expenses.xml");
            //XmlToMongoDbAndSqlServer.SaveToMSSqlServer(@"D:\Projects\TeamMojito\SupermarketsDatabaseClient\Vendors-Expenses.xml");
            VendorsTotalReport.SomeMethod("mongodb://localhost/");
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