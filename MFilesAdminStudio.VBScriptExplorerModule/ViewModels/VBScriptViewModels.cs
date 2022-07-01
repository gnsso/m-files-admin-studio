using MFilesAdminStudio.CoreModule.ViewModels;
using MFilesAdminStudio.Proxies.Models;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.VBScriptExplorerModule.ViewModels
{
    public class VBScriptViewModel : BindableBase, ISelectableViewModel
    {
        public int Id { get; set; }
        public VBScript Model { get; set; }
        public string Title { get; set; }
        public string ListingDescription { get; set; }
        public string LongDescription { get; set; }

        public VBScriptViewModel(int id, VBScript model)
        {
            Id = id;
            Model = model;

            SetDescriptions();
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        private bool isOpened;
        public bool IsOpened
        {
            get { return isOpened; }
            set { SetProperty(ref isOpened, value); }
        }

        private void SetDescriptions()
        {
            if (Model is EventHandlerScript eventHandlerScript)
            {
                Title = eventHandlerScript.EventHandlerDescription;
                ListingDescription = $"{Model.ScriptTypeView} ⦁ {eventHandlerScript.EventHandlerTypeView}";
            }
            else if (Model is StateScript stateScript)
            {
                Title = stateScript.StateName;
                ListingDescription = $"{Model.ScriptTypeView} ⦁ {stateScript.StateScriptTypeView} ⦁ {stateScript.WorkflowName}";
            }
            else if (Model is PropertyScript propertyScript)
            {
                Title = propertyScript.PropertyName;
                ListingDescription = $"{Model.ScriptTypeView} ⦁ {propertyScript.PropertyScriptTypeView}";
            }
            else if (Model is StateTransitionScript stateTransitionScript)
            {
                Title = $"{(!string.IsNullOrWhiteSpace(stateTransitionScript.TransitionName) ? stateTransitionScript.TransitionName : "Transition " + stateTransitionScript.TransitionId.ToString())} ({stateTransitionScript.FromStateName} > {stateTransitionScript.ToStateName})";
                ListingDescription = $"{Model.ScriptTypeView} ⦁ {stateTransitionScript.WorkflowName}";
            }
        }
    }
}
