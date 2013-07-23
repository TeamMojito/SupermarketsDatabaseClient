namespace SupermarketsDatabaseClient
{
    using System;
    using System.Reflection;
    using MongoDB.Driver;
    using ExcelLibrary;
    using ExcelLibrary.SpreadSheet;

    public static class VendorsTotalReport
    {
        internal static void SomeMethod(string mongoDBConnectionString)
        {
            var mongoClient = new MongoClient(mongoDBConnectionString);
            var mongoServer = mongoClient.GetServer();

            var supermarket = mongoServer.GetDatabase("supermarketDb");
            var expenses = supermarket.GetCollection("expenses");
            var vendors = supermarket.GetCollection("vendors");
            var vendorsQuery = vendors.FindAll();
            foreach (var v in vendorsQuery)
            {
                Console.WriteLine(v.GetElement("VendorName").Value.AsString);
                var ex = v.GetElement("Expenses").Value.AsBsonDocument;
                foreach (var e in ex)
                {
                    Console.WriteLine(e.Name);
                    Console.WriteLine(e.Value);
                }
            }
            //string file = "..\\..\\..\\Products-Total-Report.xlsx";
            //Workbook workbook = new Workbook();
            //Worksheet worksheet = new Worksheet("Products-Total-Report");
            ////worksheet.Cells[0, 1] = new Cell((short)1);
            ////worksheet.Cells[2, 0] = new Cell(9999999);
            ////worksheet.Cells[3, 3] = new Cell((decimal)3.45);
            ////worksheet.Cells[2, 2] = new Cell("Text string");
            ////worksheet.Cells[2, 4] = new Cell("Second string");
            ////worksheet.Cells[4, 0] = new Cell(32764.5, "#,##0.00");
            ////worksheet.Cells[5, 1] = new Cell(DateTime.Now, @"YYYY\-MM\-DD");
            ////worksheet.Cells.ColumnWidth[0, 1] = 3000;
            //workbook.Worksheets.Add(worksheet);
            //workbook.Save(file);
        }
    }
}