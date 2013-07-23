namespace SupermarketsDatabaseClient
{
    using System;
    using System.IO;
    using System.Linq;
    using MSSQLSupermarketEntityFramework;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver;

    public static class MongoDBConnector
    {
        internal static void CreateProductsReportData(string mongoDBConnectionString)
        {
            var mongoClient = new MongoClient(mongoDBConnectionString);
            var mongoServer = mongoClient.GetServer();
            if (mongoServer.DatabaseExists("supermarketDb"))
            {
                mongoServer.DropDatabase("supermarketDb");
            }

            var supermarketDb = mongoServer.GetDatabase("supermarketDb");
            var productsReports = supermarketDb.GetCollection("productsReports");

            GenerateMongoReportsFromMSSql(productsReports);

            ExportCollectionToJson(productsReports);
        }

        private static void ExportCollectionToJson(MongoCollection<BsonDocument> productsReports)
        {
            string jsonReportsDirectory = @"..\..\..\JsonReports\";
            if (Directory.Exists(jsonReportsDirectory))
            {
                Directory.Delete(jsonReportsDirectory, true);
            }

            Directory.CreateDirectory(jsonReportsDirectory);

            var reports = productsReports.FindAll();
            foreach (var report in reports)
            {
                CreateJsonFile(jsonReportsDirectory, report);
            }
        }

        private static void CreateJsonFile(string jsonReportsDirectory, BsonDocument report)
        {
            report.Remove("_id");
            BsonValue productIdAsBson;
            report.TryGetValue("product-id", out productIdAsBson);
            using (var file = new StreamWriter(jsonReportsDirectory + productIdAsBson.AsInt32 + ".json"))
            {
                file.WriteLine(report.ToJson());
            }
        }

        private static void GenerateMongoReportsFromMSSql(MongoCollection<BsonDocument> productReports)
        {
            using (var supermarketEntities = new SupermarketEntities())
            {
                var reportsOrederedByProductId = supermarketEntities.Reports
                                                                    .Include("Product.Vendor")
                                                                    .OrderBy(p => p.ProductId);
                var firstProduct = reportsOrederedByProductId.First();
                int currentProductId = firstProduct.ProductId;
                int totalQuantitySold = 0;
                decimal totalIncome = 0;
                ProductReport productReport;
                Report lastReport = new Report();

                foreach (var report in reportsOrederedByProductId)
                {
                    if (currentProductId != report.ProductId)
                    {
                        productReport = new ProductReport(currentProductId,
                            report.Product.ProductName.ToString(),
                            report.Product.Vendor.VendorName.ToString().Trim(),
                            totalQuantitySold, totalIncome);
                        totalIncome = 0;
                        totalQuantitySold = 0;
                        currentProductId = report.ProductId;
                        productReports.Insert(productReport);
                    }

                    totalIncome += report.Sum;
                    totalQuantitySold += report.Quantity;
                    lastReport = report;
                }

                productReport = new ProductReport(currentProductId,
                            lastReport.Product.ProductName.ToString(),
                            lastReport.Product.Vendor.VendorName.ToString().Trim(),
                            totalQuantitySold, totalIncome);
                productReports.Insert(productReport);
            }
        }

        class ProductReport
        {
            [BsonId]
            public ObjectId Id { get; set; }

            [BsonElement("product-id")]
            public int ProductId { get; set; }

            [BsonElement("product-name")]
            public string Name { get; set; }

            [BsonElement("vendor-name")]
            public string VendorName { get; set; }

            [BsonElement("total-quantity-sold")]
            public int TotalQuantitySold { get; set; }

            [BsonElement("total-incomes")]
            public decimal TotalIncomes { get; set; }

            public ProductReport(int productId, string name, string vendorName, int totalQuantitySold, decimal totalIncomes)
            {
                this.ProductId = productId;
                this.Name = name;
                this.VendorName = vendorName;
                this.TotalQuantitySold = totalQuantitySold;
                this.TotalIncomes = totalIncomes;
            }
        }
    }
}