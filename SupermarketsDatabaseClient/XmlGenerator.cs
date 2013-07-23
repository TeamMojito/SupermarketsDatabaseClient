namespace SupermarketsDatabaseClient
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Xml.Linq;
    using MSSQLSupermarketEntityFramework;

    public static class XmlGenerator
    {
        internal static void GenerateXmlByVendors()
        {
            using (var entity = new SupermarketEntities())
            {
                string xmlReportDirectory = "..\\..\\..\\sales-xml.xml";
                if (File.Exists(xmlReportDirectory))
                {
                    File.Delete(xmlReportDirectory);
                }
                XElement rootElement = new XElement("sales");

                var orderedByVendors = entity.Reports
                                             .Include("Product.Vendor")
                                             .OrderBy(v => v.Product.Vendor.VendorName);
                var firstReport = orderedByVendors.First();
                string currentVendorName = firstReport.Product.Vendor.VendorName.ToString().Trim();
                XElement currentVendor = new XElement("sale", new XAttribute("vendor",
                    firstReport.Product.Vendor.VendorName.Trim()));

                foreach (var currentSaleReport in orderedByVendors)
                {
                    if (currentVendorName != currentSaleReport.Product.Vendor.VendorName.Trim())
                    {
                        rootElement.Add(currentVendor);
                        currentVendor = new XElement("sale", new XAttribute("vendor",
                            currentSaleReport.Product.Vendor.VendorName.Trim()));
                       currentVendorName = currentSaleReport.Product.Vendor.VendorName.Trim();
                    }
                    
                    XElement currentVendorSummary = new XElement("summary",
                        new XAttribute("date", string.Format("{0}", currentSaleReport.ReportDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture))),
                        new XAttribute("total-sum", string.Format("{0:f2}", currentSaleReport.Sum)));
                    currentVendor.Add(currentVendorSummary);
                    
                }
                rootElement.Add(currentVendor);
                rootElement.Save(xmlReportDirectory);
            }
        }
    }
}