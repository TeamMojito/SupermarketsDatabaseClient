﻿using System.IO;
using ExtractExcelReportsFromZip;

namespace SupermarketsDatabaseClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //ZipExtractor zipExtractor = new ZipExtractor(@"D:\Trash\Sample-Sales-Reports.zip");
            //var directory = zipExtractor.ExtractContentTo(@"D:\Trash\ExcelReports");
            ExcelToMySqlParser excelToMySqlParser = new ExcelToMySqlParser(@"D:\Trash\ExcelReports");
            excelToMySqlParser.NemogaDaTiIzmislqImeSq();
        }
    }
}
