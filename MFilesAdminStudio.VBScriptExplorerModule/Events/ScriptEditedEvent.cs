using MFilesAdminStudio.VBScriptExplorerModule.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.VBScriptExplorerModule.Events
{
    public class ScriptEditedEvent : PubSubEvent<VBScriptTabViewModel>
    {
    }
}
