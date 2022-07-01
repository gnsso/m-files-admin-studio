using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.CoreModule
{
    public class AppSession
    {
        public string FileVersion { get; set; }
        public bool IsPreRelease => FileVersion?.StartsWith("0") ?? false;
    }
}
