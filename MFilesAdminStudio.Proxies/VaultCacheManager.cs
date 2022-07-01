using MFilesAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Proxies
{
    public class VaultCacheManager
    {
        private readonly Vault vault;

        private Dictionary<int, string> stateNames;
        private Dictionary<int, string> workflowNames;
        private Dictionary<int, string> propertyNames;

        public VaultCacheManager(Vault vault)
        {
            this.vault = vault;
        }

        public string GetStateName(int stateId)
        {
            if (stateNames == null)
            {
                stateNames = vault.ValueListItemOperations.GetValueListItems((int)MFBuiltInValueList.MFBuiltInValueListStates, false).Cast<ValueListItem>().ToDictionary(d => d.ID, d => d.Name);
            }

            if (stateId == 0)
            {
                return "Initial State";
            }

            return stateNames[stateId];
        }

        public string GetWorkfloName(int workflowId)
        {
            if (workflowNames == null)
            {
                workflowNames = vault.ValueListItemOperations.GetValueListItems((int)MFBuiltInValueList.MFBuiltInValueListWorkflows, false).Cast<ValueListItem>().ToDictionary(d => d.ID, d => d.Name);
            }

            return workflowNames[workflowId];
        }

        public string GetPropertyName(int propertyId)
        {
            if (propertyNames == null)
            {
                propertyNames = vault.PropertyDefOperations.GetPropertyDefs().Cast<PropertyDef>().ToDictionary(d => d.ID, d => d.Name);
            }

            return propertyNames[propertyId];
        }
    }
}
