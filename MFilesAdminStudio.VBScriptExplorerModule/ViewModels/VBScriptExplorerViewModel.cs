using MFilesAdminStudio.CoreModule;
using MFilesAdminStudio.CoreModule.Converters;
using MFilesAdminStudio.CoreModule.ViewModels;
using MFilesAdminStudio.Proxies;
using MFilesAdminStudio.Proxies.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using Unity;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;
using static MFilesAdminStudio.CoreModule.Helpers.StaticFunctions;
using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.VBScriptExplorerModule.Events;
using MFilesAdminStudio.CoreModule.Helpers;
using ICSharpCode.AvalonEdit;

namespace MFilesAdminStudio.VBScriptExplorerModule.ViewModels
{
    public class VBScriptExplorerViewModel : ModuleViewModelBase
    {
        private readonly IRegionManager regionManager;

        private bool anyScriptTabsOpened;
        public bool AnyScriptTabsOpened
        {
            get { return anyScriptTabsOpened; }
            set { SetProperty(ref anyScriptTabsOpened, value); }
        }

        private ObservableCollection<VBScriptTabViewModel> scriptTabs;
        public ObservableCollection<VBScriptTabViewModel> ScriptTabs
        {
            get { return scriptTabs; }
            set { SetProperty(ref scriptTabs, value); }
        }

        private VBScriptTabViewModel selectedTab;
        public VBScriptTabViewModel SelectedTab
        {
            get { return selectedTab; }
            set 
            {
                if (selectedTab != null)
                {
                    selectedTab.PropertyChanged -= TabPropertyChanged;
                }

                SetProperty(ref selectedTab, value);

                if (selectedTab != null)
                {
                    selectedTab.PropertyChanged += TabPropertyChanged;
                }
            }
        }

        private VBScriptsListingViewModel listing;
        public VBScriptsListingViewModel Listing
        {
            get { return listing; }
            set { SetProperty(ref listing, value); }
        }

        public DelegateCommand<VBScriptViewModel> OpenTab { get; set; }
        public DelegateCommand<VBScriptTabViewModel> CloseTab { get; set; }
        public DelegateCommand CloseAllTabs { get; set; }
        public DelegateCommand SaveScripts { get; set; }

        public VBScriptExplorerViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, CR.VBScriptExplorer_ModuleName)
        {
            this.regionManager = regionManager;

            Listing = new VBScriptsListingViewModel(unityContainer);

            ScriptTabs = new ObservableCollection<VBScriptTabViewModel>();
            ScriptTabs.CollectionChanged += delegate { AnyScriptTabsOpened = ScriptTabs.Any(); };

            OpenTab = new DelegateCommand<VBScriptViewModel>(ExecuteOpenTab);
            CloseTab = new DelegateCommand<VBScriptTabViewModel>(ExecuteCloseTab);
            CloseAllTabs = new DelegateCommand(ExecuteCloseAllTabs);
            SaveScripts = new DelegateCommand(ExecuteSaveScripts);

            EventOperator.GetEvent<VBScriptOpenedEvent>().Subscribe(ExecuteOpenTab);
            EventOperator.GetEvent<ConnectedVaultChangedEvent>().Subscribe(ExeucuteConnectedVaultChanged);
            EventOperator.GetEvent<VBScriptSaveRequestedEvent>().Subscribe(ExecuteVBScriptSaveRequest);
            EventOperator.GetEvent<VBScriptReloadRequestedEvent>().Subscribe(ExecuteVBScriptReloadRequest);
        }

        private void ExeucuteConnectedVaultChanged()
        {
            ExecuteCloseAllTabs();

            Listing.LoadScripts();
        }

        private void ExecuteOpenTab(VBScriptViewModel scriptVM)
        {
            scriptVM.IsOpened = true;

            if (ScriptTabs.Any(a => a.Id == scriptVM.Id))
            {
                var scriptTab = ScriptTabs.Single(s => s.Id == scriptVM.Id);

                SelectedTab = scriptTab;
            }
            else
            {
                ScriptTabs.Add(new VBScriptTabViewModel
                {
                    Id = scriptVM.Id,
                    Header = scriptVM.Title,
                    Model = scriptVM.Model,
                    TooltipHeader = $"{scriptVM.Title}{(scriptVM.Model.Enabled ? "" : " (disabled)")}",
                    Description = scriptVM.ListingDescription,
                });

                SelectedTab = ScriptTabs.Last();
            }
        }

        private void TabPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(VBScriptTabViewModel.FindText))
            {
                EventOperator.GetEvent<VBScriptFindTextChangedEvent>().Publish(sender as VBScriptTabViewModel);
            }
        }

        private async void ExecuteCloseTab(VBScriptTabViewModel tab)
        {
            var scriptTabIndex = -1;

            if (tab.IsEditedOriginalScript)
            {
                if (!await SimpleMessageDialog.ConfirmNormal("There are unsaved changes, do you want to continue?", dialog: CR.Main_RootDialogIdentifier))
                {
                    // to re-add tab, becuse cant handle vbscriptbox's default close tab behaviour
                    scriptTabIndex = ScriptTabs.IndexOf(tab);
                }
            }

            ScriptTabs.Remove(tab);

            EventOperator.GetEvent<VBScriptTabClosedEvent>().Publish(tab.Id);

            if (scriptTabIndex >= 0)
            {
                ScriptTabs.Insert(scriptTabIndex, tab);

                SelectedTab = tab;
            }
        }

        private async void ExecuteCloseAllTabs()
        {
            if (ScriptTabs.Any(a => a.IsEditedOriginalScript))
            {
                if (await SimpleMessageDialog.ConfirmNormal("There are unsaved changes, do you want to continue?", dialog: CR.Main_RootDialogIdentifier))
                {
                    var openedTabVMIds = ScriptTabs.Select(s => s.Id).ToArray();

                    ScriptTabs.Clear();

                    foreach (var id in openedTabVMIds)
                    {
                        EventOperator.GetEvent<VBScriptTabClosedEvent>().Publish(id);
                    }
                }
            }
        }

        private void ExecuteSaveScripts()
        {

        }

        private async void ExecuteVBScriptSaveRequest((VBScript model, string script) payload)
        {
            var vault = unityContainer.Resolve<VaultProxy>();

            try
            {
                if (payload.model is StateTransitionScript stScript)
                {
                    vault.UpdateStateTransitionVBScript(stScript.WorkflowId, stScript.TransitionId, payload.script);
                }
                else if (payload.model is PropertyScript propScript)
                {
                    vault.UpdatePropertyVBScript(propScript.PropertyId, propScript.PropertyScriptType, payload.script);
                }
                else if (payload.model is EventHandlerScript ehScript)
                {
                    vault.UpdateEventHandlerVBScript(ehScript.EventHandlerGuid, payload.script);
                }
                else if (payload.model is StateScript stateScript)
                {
                    vault.UpdateStateVBScript(stateScript.WorkflowId, stateScript.StateId, stateScript.StateScriptType, payload.script);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(payload.model.GetType().ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                await SimpleMessageDialog.Error(ex.Message, CR.Main_RootDialogIdentifier);                
            }
        }

        private async void ExecuteVBScriptReloadRequest((VBScript model, TextEditor editor) payload)
        {
            try
            {
                var vault = unityContainer.Resolve<VaultProxy>();
                var vbScript = string.Empty;
                var nullKey = "<null>";

                if (payload.model is StateTransitionScript stScript)
                {
                    vbScript = vault.GetStateTransitionVBScripts().SingleOrDefault(s => s.WorkflowId == stScript.WorkflowId && s.TransitionId == stScript.TransitionId)?.Script ?? nullKey;
                }
                else if (payload.model is PropertyScript propScript)
                {
                    vbScript = vault.GetPropertyVBScripts().SingleOrDefault(s => s.PropertyId == propScript.PropertyId)?.Script ?? nullKey;
                }
                else if (payload.model is EventHandlerScript ehScript)
                {
                    vbScript = vault.GetEventHandlerScripts().SingleOrDefault(s => s.EventHandlerGuid == ehScript.EventHandlerGuid)?.Script ?? nullKey;
                }
                else if (payload.model is StateScript stateScript)
                {
                    vbScript = vault.GetStateVBScripts().SingleOrDefault(s => s.WorkflowId == stateScript.WorkflowId && s.StateId == stateScript.StateId)?.Script ?? nullKey;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(payload.model.GetType().ToString());
                }

                if (vbScript == nullKey)
                {
                    throw new ArgumentException("Script not found on server");
                }

                payload.editor.Text = payload.model.Script = vbScript;
                payload.editor.Document.UndoStack.ClearAll();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                await SimpleMessageDialog.Error(ex.Message, CR.Main_RootDialogIdentifier);
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Listing.LoadScripts();
        }
    }
}
