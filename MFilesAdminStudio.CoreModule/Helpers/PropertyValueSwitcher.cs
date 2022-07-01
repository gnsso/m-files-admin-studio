using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MFilesAdminStudio.CoreModule.Helpers
{
    public class PropertyValueSwitcher
    {
        private readonly IDictionary<string, List<Action>> statedActions = new Dictionary<string, List<Action>>();

        public PropertyValueSwitcher SetFor(string state, List<Action> actions)
        {
            statedActions[state] = actions;

            return this;
        }

        public void Switch(string state)
        {
            foreach (var action in statedActions[state])
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    action?.Invoke();
                }, DispatcherPriority.ContextIdle);
            }
        }
    }
}
