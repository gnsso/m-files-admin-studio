using MFilesAdminStudio.Proxies.Models;
using MFilesAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Proxies
{
    public class VaultProxy
    {
        private static readonly Dictionary<VBScriptType, string> scriptTypesWithDescriptions = Enum.GetValues(typeof(VBScriptType)).Cast<VBScriptType>().ToDictionary(d => d, d => typeof(VBScriptType).GetMember(d.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description);

        private readonly Vault vault;
        private readonly VaultCacheManager cache;

        public string Name { get; }
        public string Guid { get; }

        public VaultProxy(Vault vault)
        {
            this.vault = vault;

            Name = vault.Name;
            Guid = vault.GetGUID();

            cache = new VaultCacheManager(vault);
        }

        public List<EventHandlerScript> GetEventHandlerScripts()
        {
            var descriptions = Enum.GetValues(typeof(EventHandlerType)).Cast<EventHandlerType>().ToDictionary(d => d, d => typeof(EventHandlerType).GetMember(d.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description);

            return vault.ManagementOperations.GetEventHandlers().Cast<MFilesAPI.EventHandler>().Select(eventHandler =>
            {
                var eventHandlerType = (EventHandlerType)eventHandler.EventType;

                var vbScript = new EventHandlerScript
                {
                    EventHandlerGuid = eventHandler.GUID,
                    EventHandlerType = eventHandlerType,
                    ScriptTypeView = scriptTypesWithDescriptions[VBScriptType.EventHandler],
                    EventHandlerDescription = eventHandler.Description,
                    Script = eventHandler.VBScript,
                    Enabled = eventHandler.Active,
                    EventHandlerTypeView = descriptions[eventHandlerType],
                };

                return vbScript;
            }).OrderBy(o => (int)o.EventHandlerType).ToList();
        }

        public List<StateScript> GetStateVBScripts()
        {
            var workflows = vault.WorkflowOperations.GetWorkflowsAdmin().Cast<WorkflowAdmin>().ToList();
            var descriptions = Enum.GetValues(typeof(StateScriptType)).Cast<StateScriptType>().ToDictionary(d => d, d => typeof(StateScriptType).GetMember(d.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description);

            return workflows.SelectMany(workflow => workflow.States.Cast<StateAdmin>().SelectMany(state =>
            {
                var vbScripts = new List<StateScript>();

                if (!string.IsNullOrWhiteSpace(state.ActionRunVBScriptDefinition))
                {
                    vbScripts.Add(new StateScript
                    {
                        StateScriptType = StateScriptType.StateAction,
                        ScriptTypeView = scriptTypesWithDescriptions[VBScriptType.State],
                        Enabled = state.ActionRunVBScript,
                        Script = state.ActionRunVBScriptDefinition,
                        WorkflowId = workflow.Workflow.ID,
                        StateId = state.ID,
                        StateName = cache.GetStateName(state.ID),
                        StateScriptTypeView = descriptions[StateScriptType.StateAction],
                        WorkflowName = cache.GetWorkfloName(workflow.Workflow.ID)
                    });
                }

                if (!string.IsNullOrWhiteSpace(state.Preconditions.VBScriptDefinition))
                {
                    vbScripts.Add(new StateScript
                    {
                        StateScriptType = StateScriptType.PreCondition,
                        ScriptTypeView = scriptTypesWithDescriptions[VBScriptType.State],
                        Enabled = state.Preconditions.VBScript,
                        Script = state.Preconditions.VBScriptDefinition,
                        WorkflowId = workflow.Workflow.ID,
                        StateId = state.ID,
                        StateName = cache.GetStateName(state.ID),
                        StateScriptTypeView = descriptions[StateScriptType.PreCondition],
                        WorkflowName = cache.GetWorkfloName(workflow.Workflow.ID)
                    });
                }

                if (!string.IsNullOrWhiteSpace(state.Postconditions.VBScriptDefinition))
                {
                    vbScripts.Add(new StateScript
                    {
                        StateScriptType = StateScriptType.PostCondition,
                        ScriptTypeView = scriptTypesWithDescriptions[VBScriptType.State],
                        Enabled = state.Postconditions.VBScript,
                        Script = state.Postconditions.VBScriptDefinition,
                        WorkflowId = workflow.Workflow.ID,
                        StateId = state.ID,
                        StateName = cache.GetStateName(state.ID),
                        StateScriptTypeView = descriptions[StateScriptType.PostCondition],
                        WorkflowName = cache.GetWorkfloName(workflow.Workflow.ID)
                    });
                }

                return vbScripts;
            })).ToList();
        }

        public List<StateTransitionScript> GetStateTransitionVBScripts()
        {
            var workflows = vault.WorkflowOperations.GetWorkflowsAdmin().Cast<WorkflowAdmin>().ToList();

            return workflows.SelectMany(workflow => workflow.StateTransitions.Cast<StateTransition>().Where(w => !string.IsNullOrWhiteSpace(w.TriggerAllowedByVBScript)).Select(transition =>
            {
                var vbScript = new StateTransitionScript
                {
                    Enabled = transition.TriggerMode == MFAutoStateTransitionMode.MFASTModeAllowedByScript,
                    ScriptTypeView = scriptTypesWithDescriptions[VBScriptType.StateTransition],
                    Script = transition.TriggerAllowedByVBScript,
                    WorkflowId = workflow.Workflow.ID,
                    TransitionId = transition.ID,
                    TransitionName = transition.Name,
                    FromStateId = transition.FromState,
                    ToStateId = transition.ToState,
                    FromStateName = cache.GetStateName(transition.FromState),
                    ToStateName = cache.GetStateName(transition.ToState),
                    WorkflowName = cache.GetWorkfloName(workflow.Workflow.ID),
                };

                return vbScript;
            })).ToList();
        }

        public List<PropertyScript> GetPropertyVBScripts()
        {
            var properties = vault.PropertyDefOperations.GetPropertyDefsAdmin().Cast<PropertyDefAdmin>();
            var descriptions = Enum.GetValues(typeof(PropertyScriptType)).Cast<PropertyScriptType>().ToDictionary(d => d, d => typeof(PropertyScriptType).GetMember(d.ToString())[0].GetCustomAttribute<DescriptionAttribute>().Description);

            return properties.SelectMany(property =>
            {
                var vbScripts = new List<PropertyScript>();

                if (!string.IsNullOrWhiteSpace(property.AutomaticValue.CVVCode))
                {
                    vbScripts.Add(new PropertyScript
                    {
                        PropertyId = property.PropertyDef.ID,
                        ScriptTypeView = scriptTypesWithDescriptions[VBScriptType.Property],
                        PropertyScriptType = PropertyScriptType.CalculatedValue,
                        PropertyScriptTypeView = descriptions[PropertyScriptType.CalculatedValue],
                        Enabled = property.PropertyDef.AutomaticValueType == MFAutomaticValueType.MFAutomaticValueTypeCalculatedWithVBScript,
                        Script = property.AutomaticValue.CVVCode,
                        PropertyName = cache.GetPropertyName(property.PropertyDef.ID)
                    });
                }

                if (!string.IsNullOrWhiteSpace(property.AutomaticValue.ANVCode))
                {
                    vbScripts.Add(new PropertyScript
                    {
                        PropertyId = property.PropertyDef.ID,
                        ScriptTypeView = scriptTypesWithDescriptions[VBScriptType.Property],
                        PropertyScriptType = PropertyScriptType.AutomaticNumbering,
                        PropertyScriptTypeView = descriptions[PropertyScriptType.AutomaticNumbering],
                        Enabled = property.PropertyDef.AutomaticValueType == MFAutomaticValueType.MFAutomaticValueTypeWithVBScript,
                        Script = property.AutomaticValue.ANVCode,
                        PropertyName = cache.GetPropertyName(property.PropertyDef.ID)
                    });
                }

                if (!string.IsNullOrWhiteSpace(property.Validation.VBScript))
                {
                    vbScripts.Add(new PropertyScript
                    {
                        PropertyId = property.PropertyDef.ID,
                        ScriptTypeView = scriptTypesWithDescriptions[VBScriptType.Property],
                        PropertyScriptType = PropertyScriptType.Validation,
                        PropertyScriptTypeView = descriptions[PropertyScriptType.Validation],
                        Enabled = property.PropertyDef.ValidationType == MFValidationType.MFValidationTypeVBScript,
                        Script = property.Validation.VBScript,
                        PropertyName = cache.GetPropertyName(property.PropertyDef.ID)
                    });
                }

                return vbScripts;
            }).ToList();
        }

        public void UpdateEventHandlerVBScript(string guid, string vbScript)
        {
            var eventHandlers = vault.ManagementOperations.GetEventHandlers();
            var handler = eventHandlers.Cast<MFilesAPI.EventHandler>().SingleOrDefault(s => s.GUID == guid);

            if (handler is null)
            {
                throw new ArgumentNullException($"Event handler was not found with given values. Guid: {guid}");
            }

            handler.VBScript = vbScript;

            vault.ManagementOperations.SetEventHandlers(eventHandlers);
        }

        public void UpdateStateVBScript(int workflowId, int stateId, StateScriptType type, string vbScript)
        {
            var workflow = vault.WorkflowOperations.GetWorkflowAdmin(workflowId);
            var state = workflow.States.Cast<StateAdmin>().SingleOrDefault(s => s.ID == stateId);

            if (state is null)
            {
                throw new ArgumentNullException($"State was not found with given values. Workflow Id: {workflowId}, State Id: {stateId}");
            }

            switch (type)
            {
                case StateScriptType.StateAction:
                    state.ActionRunVBScriptDefinition = vbScript;
                    break;
                case StateScriptType.PreCondition:
                    state.Preconditions.VBScriptDefinition = vbScript;
                    break;
                case StateScriptType.PostCondition:
                    state.Postconditions.VBScriptDefinition = vbScript;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(StateScriptType));
            }

            vault.WorkflowOperations.UpdateWorkflowAdmin(workflow);
        }

        public void UpdateStateTransitionVBScript(int workflowId, int transitionId, string vbScript)
        {
            var workflow = vault.WorkflowOperations.GetWorkflowAdmin(workflowId);
            var transition = workflow.StateTransitions.Cast<StateTransition>().SingleOrDefault(s => s.ID == transitionId);

            if (transition is null)
            {
                throw new ArgumentNullException($"State transition was not found with given values. Workflow Id: {workflowId}, Transition Id: {transitionId}");
            }

            transition.TriggerAllowedByVBScript = vbScript;

            vault.WorkflowOperations.UpdateWorkflowAdmin(workflow);
        }

        public void UpdatePropertyVBScript(int propId, PropertyScriptType type, string vbScript)
        {
            var prop = vault.PropertyDefOperations.GetPropertyDefAdmin(propId);

            switch (type)
            {
                case PropertyScriptType.CalculatedValue:
                    prop.AutomaticValue.CVVCode = vbScript;
                    break;
                case PropertyScriptType.AutomaticNumbering:
                    prop.AutomaticValue.ANVCode = vbScript;
                    break;
                case PropertyScriptType.Validation:
                    prop.Validation.VBScript = vbScript;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(PropertyScriptType));
            }

            vault.PropertyDefOperations.UpdatePropertyDefAdmin(prop);
        }

        public string GetDatabase()
        {
            return vault.ManagementOperations.GetVaultProperties().SQLDatabase != null ? "SQL Server" : "Firebird";
        }

        public string GetVersion()
        {
            return vault.GetServerVersionOfVault().Display;
        }
    }
}
