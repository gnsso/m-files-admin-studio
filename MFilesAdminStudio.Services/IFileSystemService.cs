using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Services
{
    public interface IFileSystemService
    {
        string GetBaseFolderPath();
        string GetFilePath(string path);
        string GetTempFilePath(string path);
        IDisposable UseTempFile(out string path, string extension = "");
    }
}
