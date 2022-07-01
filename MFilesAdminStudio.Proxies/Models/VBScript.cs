using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Proxies.Models
{
    public class VBScript
    {
        public VBScriptType ScriptType { get; }
        public string ScriptTypeView { get; set; }
        public string Script { get; set; }
        public bool Enabled { get; set; }

        public VBScript(VBScriptType scriptType)
        {
            ScriptType = scriptType;
        }
    }

    public class EventHandlerScript : VBScript
    {
        public string EventHandlerGuid { get; set; }
        public EventHandlerType EventHandlerType { get; set; }
        public string EventHandlerTypeView { get; set; }
        public string EventHandlerDescription { get; set; }

        public EventHandlerScript() : base(VBScriptType.EventHandler)
        {
            
        }
    }

    public class PropertyScript : VBScript
    {
        public PropertyScriptType PropertyScriptType { get; set; }
        public string PropertyScriptTypeView { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }

        public PropertyScript() : base(VBScriptType.Property)
        {

        }
    }

    public class StateScript : VBScript
    {
        public StateScriptType StateScriptType { get; set; }
        public string StateScriptTypeView { get; set; }
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }

        public StateScript() : base(VBScriptType.State)
        {

        }
    }

    public class StateTransitionScript : VBScript
    {
        public int TransitionId { get; set; }
        public string TransitionName { get; set; }
        public int WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public int FromStateId { get; set; }
        public string FromStateName { get; set; }
        public int ToStateId { get; set; }
        public string ToStateName { get; set; }

        public StateTransitionScript() : base(VBScriptType.StateTransition)
        {

        }
    }

    public enum VBScriptType
    {
        [Description("Event Handler")]
        [VBScriptTypeView("EVENT HANDLERS")]
        EventHandler,

        [Description("Property")]
        [VBScriptTypeView("PROPERTIES")]
        Property,

        [Description("State")]
        [VBScriptTypeView("STATES")]
        State,

        [Description("State Transition")]
        [VBScriptTypeView("STATE TRANSITIONS")]
        StateTransition
    }

    public class VBScriptTypeViewAttribute : Attribute
    {
        public string View { get; }

        public VBScriptTypeViewAttribute(string view)
        {
            View = view;
        }
    }
}
