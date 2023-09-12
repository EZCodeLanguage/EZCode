namespace PackageSystem
{
    using System.IO;
    using System.IO.Compression;
    public static class PackageHandler
    {
        public static void CreatePackage(string inputFolderPath, string outputFilePath)
        {
            if (File.Exists(outputFilePath)) File.Delete(outputFilePath);
            ZipFile.CreateFromDirectory(inputFolderPath, outputFilePath);
        }

        public static void extractPackage(string output, string location)
        {
            if (!Directory.Exists(location)) Directory.CreateDirectory(location);
            if (File.Exists(output))
            {
                ZipFile.ExtractToDirectory(output, location);
            }
        }

        public static void deleteDirectory(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        public static void deleteFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }
}