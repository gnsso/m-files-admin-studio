using MFilesAdminStudio.Proxies.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.VBScriptExplorerModule.ViewModels
{
    public class VBScriptScopeViewModel : BindableBase
    {
        private string scopeName;
        public string ScopeName
        {
            get { return scopeName; }
            set { SetProperty(ref scopeName, value); }
        }

        private VBScriptType? scriptType;
        public VBScriptType? ScriptType
        {
            get { return scriptType; }
            set { SetProperty(ref scriptType, value); }
        }
    }
}
