using MaterialDesignExtensions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.CoreModule.Models
{
    public class MainModuleNavParams
    {
        public bool IsAppBarVisible { get; set; }
        public bool IsNavBarButtonVisible { get; set; }
        public string AppBarTitle { get; set; }
        public string AppBarSubTitle { get; set; }
        public List<INavigationItem> NavItems { get; set; }
        public string InitModuleName { get; set; }
        public (string key, object value) InitModuleParam { get; set; }
    }
}
