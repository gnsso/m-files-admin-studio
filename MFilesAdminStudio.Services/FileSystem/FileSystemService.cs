using MFilesAdminStudio.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Services.FileSystem
{
    public class FileSystemService : IFileSystemService
    {
        private readonly string appDataFolder;
        private readonly string companyFolder;
        private readonly string productFolder;
        private readonly string tempFolder;

        public FileSystemService(string baseFolderPath, string companyName, string productName)
        {
            appDataFolder = baseFolderPath;
            companyFolder = Path.Combine(appDataFolder, companyName);
            productFolder = Path.Combine(companyFolder, productName);
            tempFolder = Path.Combine(productFolder, "temp");
        }

        public string GetBaseFolderPath()
        {
            return productFolder;
        }

        public string GetFilePath(string path)
        {
            CheckFolders();

            return Path.Combine(productFolder, path);
        }

        public string GetTempFilePath(string path)
        {
            CheckFolders();

            return Path.Combine(tempFolder, path);
        }

        public void CleanTempFolder()
        {
            try
            {
                var files = Directory.GetFiles(tempFolder);

                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
            catch
            {

            }
        }

        public IDisposable UseTempFile(out string path, string extension = "")
        {
            var tempFile = AutoTempFile.Create(extension);

            path = tempFile.Path;

            return tempFile;
        }

        private void CheckFolders()
        {
            if (!Directory.Exists(companyFolder))
            {
                Directory.CreateDirectory(companyFolder);

                while (!Directory.Exists(companyFolder))
                {
                    Thread.Sleep(20);
                }
            }

            if (!Directory.Exists(productFolder))
            {
                Directory.CreateDirectory(productFolder);

                while (!Directory.Exists(productFolder))
                {
                    Thread.Sleep(20);
                }
            }

            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);

                while (!Directory.Exists(tempFolder))
                {
                    Thread.Sleep(20);
                }
            }
        }
    }
}
