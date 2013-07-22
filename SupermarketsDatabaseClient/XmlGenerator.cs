namespace SupermarketsDatabaseClient
{
    using System;
    using System.Linq;
    using System.Xml;
    using System.IO;
    using System.Text;

    public static class XmlGenerator
    {
        internal static void GenerateXmlByVendors()
        {
            StringBuilder output = new StringBuilder();

            String xmlString =
                @"<?xml version=""1.0"" encoding=""utf-8"">
                <Items>
                    <Item>test with a child element <more/> stuff</Item>
                </Items>";
        }
    }
}