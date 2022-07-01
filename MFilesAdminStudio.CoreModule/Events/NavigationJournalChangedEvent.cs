using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.CoreModule.Events
{
    public class NavigationJournalChangedEvent : PubSubEvent<NavigationJournalParams>
    {

    }

    public class NavigationJournalParams
    {
        public bool CanGoBack { get; set; }
        public bool IsGoBackVisible { get; set; }
        public bool CanGoForward { get; set; }
        public bool IsGoForwardVisible { get; set; }
    }
}
