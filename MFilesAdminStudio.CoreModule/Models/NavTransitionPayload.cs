using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MFilesAdminStudio.CoreModule.Models
{
    public class NavTransitionPayload
    {
        public string FromRegion { get; set; }
        public string ToRegion { get; set; }
        public string FromView { get; set; }
        public string ToView { get; set; }
        public object CurrentView { get; set; }
        public Func<Dispatcher, (string key, object value)> Function { get; set; }
    }
}
