using ExtractExcelReportsFromZip;

namespace SupermarketsDatabaseClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ZipExtractor zipExtractor = new ZipExtractor(@"D:\Trash\Sample-Sales-Reports.zip");
            zipExtractor.ExtractContentTo(@"D:\Trash\Reports");
        }
    }
}
