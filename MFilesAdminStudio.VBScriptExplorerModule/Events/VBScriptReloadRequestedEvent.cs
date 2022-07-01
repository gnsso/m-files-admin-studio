using ICSharpCode.AvalonEdit;
using MFilesAdminStudio.Proxies.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.VBScriptExplorerModule.Events
{
    public class VBScriptReloadRequestedEvent : PubSubEvent<(VBScript model, TextEditor editor)>
    {
    }
}
