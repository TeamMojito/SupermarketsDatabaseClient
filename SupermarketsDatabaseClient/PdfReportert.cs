namespace SupermarketsDatabaseClient
{
    using System;
    using System.IO;
    using System.Linq;
    using MSSQLSupermarketEntityFramework;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public static class PdfReportert
    {
        public static void CreateReport()
        {
            string directoryCreating = "..\\..\\..\\SalesReports.pdf";
            if (File.Exists(directoryCreating))
            {
                File.Delete(directoryCreating);
            }
            Document pdfReport = new Document();
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfReport,
                new System.IO.FileStream(directoryCreating, System.IO.FileMode.Create));

            pdfReport.Open();
            using (pdfReport)
            {
                //AddParagraph(pdfReport, iTextSharp.text.Element.ALIGN_CENTER, 
                //    new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD, BaseColor.BLACK),
                //    new Chunk("\n\n\nWhat kind of paper is the best for making paper airplanes?\n\n\n\n\n")); 
                PdfPTable reportTable = new PdfPTable(5);
                AddCellToTable(reportTable, "Aggregated Sales Report", 1, 5);

                using (var supermarketEntities = new SupermarketEntities())
                {
                    var reportsOrderedByDate = supermarketEntities.Reports.OrderBy(x => x.ReportDate);
                    var currentDate = reportsOrderedByDate.First().ReportDate;
                    decimal currentSum = 0;
                    decimal totalSum = 0;
                    AddHeader(reportTable, currentDate);
                    // aligment  0 = left, 1 = center, 2 = right

                    foreach (var report in reportsOrderedByDate)
                    {
                        if (currentDate != report.ReportDate)
                        {
                            AddFooter(reportTable, "Total sum for " + currentDate.ToString(), currentSum);
                            currentSum = 0;
                            currentDate = report.ReportDate;
                            AddHeader(reportTable, currentDate);
                        }
                        else
                        {

                            AddCellToTable(reportTable, report.Product.ProductName.ToString(), 0);
                            AddCellToTable(reportTable, report.Quantity.ToString(), 0);
                            AddCellToTable(reportTable, report.UnitPrice.ToString(), 0);
                            AddCellToTable(reportTable, report.Supermarket.ToString(), 0);
                            AddCellToTable(reportTable, report.Sum.ToString(), 0);
                            totalSum += (decimal)report.Sum;
                            currentSum += (decimal)report.Sum;
                        }
                    }

                    AddFooter(reportTable, "Total sum for " + currentDate.ToString(), currentSum);
                    AddFooter(reportTable, "Grand total:", totalSum);
                }

                pdfReport.Add(reportTable);
            }
        }

        private static void AddFooter(PdfPTable reportTable, string cellInfo, decimal sum)
        {
            AddCellToTable(reportTable, cellInfo, 2, 4);
            AddCellToTable(reportTable, sum.ToString(), 2);
        }

        private static void AddHeader(PdfPTable reportTable, DateTime currentDate)
        {
            AddCellToTable(reportTable, "Date: " + currentDate.ToString(), 0, 5);
            AddCellToTable(reportTable, "Product", 0);
            AddCellToTable(reportTable, "Quantity", 0);
            AddCellToTable(reportTable, "Unit Price", 0);
            AddCellToTable(reportTable, "Location ", 0);
            AddCellToTable(reportTable, "Sum", 0);
        }

        private static void AddCellToTable(PdfPTable reportTable, string text, int aligment, int colspan = 1, bool isHeader = false)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text));
            cell.Colspan = colspan;
            cell.HorizontalAlignment = aligment;
            if (isHeader)
            {
                //cell.BackgroundColor = new Color(0, 150, 0);
            }
            reportTable.AddCell(cell);
        }

        public static void AddParagraph(Document doc, int alignment, iTextSharp.text.Font font, iTextSharp.text.IElement content)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.SetLeading(0f, 1.2f);
            paragraph.Alignment = alignment;
            paragraph.Font = font;
            paragraph.Add(content);
            doc.Add(paragraph);
        }
    }
}