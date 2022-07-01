using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.CoreModule.Events
{
    public static class EventOperator
    {
        private static readonly IEventAggregator eventAggregator;

        static EventOperator()
        {
            eventAggregator = new EventAggregator();
        }

        public static T GetEvent<T>() where T : EventBase, new()
        {
            return eventAggregator.GetEvent<T>();
        }
    }
}
