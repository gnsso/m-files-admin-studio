using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.CoreModule.Helpers;
using MFilesAdminStudio.CoreModule.ViewModels;
using MFilesAdminStudio.Proxies;
using MFilesAdminStudio.Proxies.Models;
using MFilesAdminStudio.VBScriptExplorerModule.Events;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace MFilesAdminStudio.VBScriptExplorerModule.ViewModels
{
    public class VBScriptsListingViewModel : InteractiveViewModel
    {
        private readonly IUnityContainer unityContainer;
        private readonly string scriptsLoadingState = "scripts-loading";
        private readonly string scriptsLoadedState = "scripts-loaded";
        
        private string title = "VAULT SCRIPTS";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private List<VBScriptViewModel> vbScriptVMsSource;
        private ViewRequestToken vbScriptsRequestToken;

        private ICollectionView vbScripts;
        public ICollectionView VBScripts
        {
            get { return vbScripts; }
            set { SetProperty(ref vbScripts, value); }
        }

        private VBScriptsListingFilterViewModel filter = new VBScriptsListingFilterViewModel();
        public VBScriptsListingFilterViewModel Filter
        {
            get { return filter; }
            set { SetProperty(ref filter, value); }
        }

        private Visibility loaderVisibility = Visibility.Collapsed;
        public Visibility LoaderVisibility
        {
            get { return loaderVisibility; }
            set { SetProperty(ref loaderVisibility, value); }
        }

        private string loaderText;
        public string LoaderText
        {
            get { return loaderText; }
            set { SetProperty(ref loaderText, value); }
        }

        private Visibility scriptsContentVisibility = Visibility.Collapsed;
        public Visibility ScriptsContentVisibility
        {
            get { return scriptsContentVisibility; }
            set { SetProperty(ref scriptsContentVisibility, value); }
        }

        private Visibility loadMoreButtonVisibility = Visibility.Collapsed;
        public Visibility LoadMoreButtonVisibility
        {
            get { return loadMoreButtonVisibility; }
            set { SetProperty(ref loadMoreButtonVisibility, value); }
        }

        private int loadMoreItemsCount;
        public int LoadMoreItemsCount
        {
            get { return loadMoreItemsCount; }
            set { SetProperty(ref loadMoreItemsCount, value); }
        }

        private bool isFilterEnabled = false;
        public bool IsFilterEnabled
        {
            get { return isFilterEnabled; }
            set { SetProperty(ref isFilterEnabled, value); }
        }

        private string info;
        public string Info
        {
            get { return info; }
            set { SetProperty(ref info, value); }
        }

        private string triggerKeyScrollToTop;
        public string TriggerKeyScrollToTop
        {
            get { return triggerKeyScrollToTop; }
            set { SetProperty(ref triggerKeyScrollToTop, value); }
        }

        public DelegateCommand LoadVaultCommand { get; set; }
        public DelegateCommand ReloadScriptsCommand { get; set; }
        public DelegateCommand<VBScriptViewModel> OpenVBScriptCommand { get; set; }
        public DelegateCommand<VBScriptViewModel> SelectVBScriptCommand { get; set; }
        public DelegateCommand LoadMoreItemsCommand { get; set; }

        public VBScriptsListingViewModel(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;

            Filter.Changed += FilterChanged;

            EventOperator.GetEvent<VBScriptOpenedEvent>().Subscribe(VBScriptSelected);
            EventOperator.GetEvent<VBScriptsLoadingEvent>().Subscribe(VBScriptsLoading);
            EventOperator.GetEvent<VBScriptTabClosedEvent>().Subscribe(VBScriptTabClosed);

            ReloadScriptsCommand = new DelegateCommand(ExecuteReloadScripts);
            OpenVBScriptCommand = new DelegateCommand<VBScriptViewModel>(ExecuteOpenVBScript);
            SelectVBScriptCommand = new DelegateCommand<VBScriptViewModel>(ExecuteSelectVBScript);
            LoadMoreItemsCommand = new DelegateCommand(ExecuteLoadMoreItems);

            ValueSwitcher
                .SetFor(scriptsLoadingState, new List<Action>
                {
                    { () => LoaderVisibility = Visibility.Visible },
                    { () => ScriptsContentVisibility = Visibility.Collapsed },
                    { () => IsFilterEnabled = false }
                })
                .SetFor(scriptsLoadedState, new List<Action>
                {
                    { () => LoaderVisibility = Visibility.Collapsed },
                    { () => ScriptsContentVisibility = Visibility.Visible },
                    { () => IsFilterEnabled = true },
                    { () => LoadMoreButtonVisibility = (vbScriptsRequestToken?.HasMoreItems ?? false) ? Visibility.Visible : Visibility.Collapsed }
                });
        }

        private void ExecuteReloadScripts()
        {
            LoadScripts();
        }

        private void VBScriptTabClosed(int id)
        {
            vbScriptVMsSource.Single(s => s.Id == id).IsOpened = false;
        }

        private void ExecuteLoadMoreItems()
        {
            LoaderText = "Loading more scripts...";
            EventOperator.GetEvent<VBScriptsLoadingEvent>().Publish(true);

            Task.Run(() =>
            {
                VBScripts = Collections.GetView(vbScriptsRequestToken);

                LoadMoreItemsCount = vbScriptsRequestToken.MoreItemsCount;

                EventOperator.GetEvent<VBScriptsLoadingEvent>().Publish(false);
            });
        }

        private void FilterChanged(object sender, Func<object, bool> predicate)
        {
            LoaderText = "Scripts loading...";
            EventOperator.GetEvent<VBScriptsLoadingEvent>().Publish(true);

            Task.Delay(100).ContinueWith(t =>
            {
                vbScriptsRequestToken.Predicate = predicate;
                vbScriptsRequestToken.LoadedCount = 0;

                VBScripts = Collections.GetView(vbScriptsRequestToken);

                LoadMoreItemsCount = vbScriptsRequestToken.MoreItemsCount;

                Info = $"Found {vbScriptsRequestToken.TotalCount} scripts";

                EventOperator.GetEvent<VBScriptsLoadingEvent>().Publish(false);

                if (sender is string propertyName)
                {
                    switch (propertyName)
                    {
                        case nameof(Filter.SearchText):
                            Filter.IsSearchTextFocused = true;
                            break;
                    }
                }
            });
        }

        private void ExecuteChangeVault()
        {

        }

        private void ExecuteOpenVBScript(VBScriptViewModel vbScript)
        {
            SingleListItemSelector.SelectSingle(vbScriptVMsSource, vbScript);

            EventOperator.GetEvent<VBScriptOpenedEvent>().Publish(vbScript);
        }

        private void ExecuteSelectVBScript(VBScriptViewModel vbScript)
        {
            vbScript.IsSelected = !vbScript.IsSelected;
        }

        public void LoadScripts()
        {
            var vaultProxy = unityContainer.Resolve<VaultProxy>();

            Title = $"{vaultProxy.Name} VAULT SCRIPTS";

            EventOperator.GetEvent<VBScriptsLoadingEvent>().Publish(true);
            Filter.Clear(false);

            Task.Delay(100).ContinueWith(t =>
            {
                var scripts = new List<VBScript>();

                LoaderText = "Loading event handlers...";
                scripts.AddRange(vaultProxy.GetEventHandlerScripts());

                LoaderText = "Loading properties...";
                scripts.AddRange(vaultProxy.GetPropertyVBScripts());

                LoaderText = "Loading states...";
                scripts.AddRange(vaultProxy.GetStateVBScripts());

                LoaderText = "Loading state transitions...";
                scripts.AddRange(vaultProxy.GetStateTransitionVBScripts());

                Info = $"Found in total {scripts.Count} scripts";

                vbScriptVMsSource = scripts.Select((s, i) => new VBScriptViewModel(i, s)).ToList();

                vbScriptsRequestToken = Collections.SetSource(nameof(VBScripts), vbScriptVMsSource);

                vbScriptsRequestToken.Predicate = Filter.GeneratePredicate();
                vbScriptsRequestToken.IsPartialView = true;
                vbScriptsRequestToken.CountPerRequest = 50;

                VBScripts = Collections.GetView(vbScriptsRequestToken);

                TriggerKeyScrollToTop = Guid.NewGuid().ToString();

                LoadMoreItemsCount = vbScriptsRequestToken.MoreItemsCount;

                EventOperator.GetEvent<VBScriptsLoadingEvent>().Publish(false);
            });
        }

        private void VBScriptsLoading(bool isLoading)
        {
            if (isLoading)
            {
                ValueSwitcher.Switch(scriptsLoadingState);
            }
            else
            {
                ValueSwitcher.Switch(scriptsLoadedState);
            }
        }


        private void VBScriptSelected(VBScriptViewModel vbScript)
        {
            var sourceTypesList = Collections.GetSource<VBScriptViewModel>(nameof(VBScripts));

            SingleListItemSelector.SelectSingle(sourceTypesList, vbScript);
        }
    }
}
