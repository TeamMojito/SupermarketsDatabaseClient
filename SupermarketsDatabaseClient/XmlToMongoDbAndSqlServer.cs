namespace SupermarketsDatabaseClient
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Xml;
    using MSSQLSupermarketEntityFramework;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver;
    
    public static class XmlToMongoDbAndSqlServer
    {
        public static void SaveToMongoDb(string connecitonString, string xmlLocation)
        {
            var mongoClient = new MongoClient(connecitonString);
            var mongoServer = mongoClient.GetServer();
            if (mongoServer.DatabaseExists("supermarketDb"))
            {
                mongoServer.DropDatabase("supermarketDb");
            }

            var supermarketDb = mongoServer.GetDatabase("supermarketDb");
            var vendors = supermarketDb.GetCollection("vendors");

            using (XmlReader reader = XmlReader.Create(xmlLocation))
            {
                while (reader.Read())
                {
                    string vendorName = null;

                    MongoCollection expenses = supermarketDb.GetCollection("expenses");

                    if (reader.IsStartElement())
                    {
                        if (reader.Name == "sale")
                        {
                            vendorName = reader["vendor"];
                        }

                        if (reader.Name == "expenses")
                        {
                            DateTime expenseDate = DateTime.Parse(reader["month"]);
                            reader.Read();
                            decimal money = decimal.Parse(reader.Value);
                            var expense = new ExpneseMongo(expenseDate, money);
                            expenses.Insert(expense);
                        }
                    }

                    if (!string.IsNullOrEmpty(vendorName))
                    {
                        vendors.Insert(new VendorMongo(vendorName, expenses));
                    }
                }
            }
        }

        public static void SaveToMSSqlServer(string xmlLocation)
        {
            using (var supermarketEntities = new SupermarketEntities())
            {
                using (XmlReader reader = XmlReader.Create(xmlLocation))
                {
                    int vendorId = 0;
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "sale")
                            {
                                string vendorName = reader["vendor"];

                                var vendorQuery = from v in supermarketEntities.Vendors
                                                  where v.VendorName == vendorName
                                                  select v.VendorId;
                                
                                foreach (var v in vendorQuery)
                                {
                                    vendorId = v;
                                }
                            }

                            if (reader.Name == "expenses")
                            {
                                DateTime expenseDate = DateTime.Parse(reader["month"]);
                                reader.Read();
                                decimal money = decimal.Parse(reader.Value);
                                Vendor vendor = supermarketEntities.Vendors.Find(vendorId);
                                var expense = new Expens()
                                {
                                    Vendor = vendor,
                                    Month = expenseDate,
                                    Sum = money
                                };

                                supermarketEntities.Expenses.Add(expense);
                            }
                        }
                    }
                    supermarketEntities.SaveChanges();
                }
            }
        }
        
        class VendorMongo
        {
            [BsonId]
            public ObjectId Id { get; set; }

            public string VendorName { get; set; }

            public MongoCollection Expenses { get; set; }

            public VendorMongo(string vendorName, MongoCollection expenses)
            {
                this.VendorName = vendorName;
                this.Expenses = expenses;
            }
        }

        class ExpneseMongo
        {
            public DateTime Date { get; set; }

            public decimal Money { get; set; }

            public ExpneseMongo(DateTime date, decimal money)
            {
                this.Date = date;
                this.Money = money;
            }
        }
    }
}