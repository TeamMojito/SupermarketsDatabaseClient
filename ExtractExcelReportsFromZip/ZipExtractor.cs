namespace ExtractExcelReportsFromZip
{
    using System;
    using System.IO;
    using System.IO.Compression;

    public class ZipExtractor
    {
        private string extractDirectory;

        public ZipExtractor(string extractionDirectory)
        {
            this.ExtractDirectory = extractionDirectory;
        }

        public string ExtractContentTo(string directoryToExtract)
        {
            if (!Directory.Exists(directoryToExtract))
            {
                Directory.CreateDirectory(directoryToExtract);
            }

            ZipFile.ExtractToDirectory(this.ExtractDirectory, directoryToExtract);

            return directoryToExtract;
        }

        public string ExtractDirectory
        {
            get
            {
                return this.extractDirectory;
            }
            set
            {
                this.extractDirectory = value;
            }
        }
    }
}
