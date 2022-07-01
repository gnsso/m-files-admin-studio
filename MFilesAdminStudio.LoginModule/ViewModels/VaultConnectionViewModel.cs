using MFilesAdminStudio.CoreModule;
using MFilesAdminStudio.CoreModule.Controls;
using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.CoreModule.Models;
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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Unity;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.LoginModule.ViewModels
{
    public class VaultConnectionViewModel : ModuleViewModelBase
    {
        private readonly IRegionManager regionManager;

        private List<VaultViewModel> vaultsSource;

        private ICollectionView vaults;
        public ICollectionView Vaults
        {
            get { return vaults; }
            set { SetProperty(ref vaults, value); }
        }

        private VaultViewModel selectedVault;
        public VaultViewModel SelectedVault
        {
            get { return selectedVault; }
            set
            {
                SetProperty(ref selectedVault, value);

                if (value != null)
                {
                    VaultSelected(value.Guid);
                }

                IsLoginEnabled = value != null && value.IsOnline;
            }
        }

        private bool isFilterEnabled = true;
        public bool IsFilterEnabled
        {
            get { return isFilterEnabled; }
            set { SetProperty(ref isFilterEnabled, value); }
        }

        private VaultFilterViewModel filter;
        public VaultFilterViewModel Filter
        {
            get { return filter; }
            set { SetProperty(ref filter, value); }
        }

        private string triggerKeyScrollToTop;
        public string TriggerKeyScrollToTop
        {
            get { return triggerKeyScrollToTop; }
            set { SetProperty(ref triggerKeyScrollToTop, value); }
        }

        private bool isLoginEnabled = false;
        public bool IsLoginEnabled
        {
            get { return isLoginEnabled; }
            set { SetProperty(ref isLoginEnabled, value); }
        }

        public DelegateCommand<VaultViewModel> SelectVaultCommand { get; set; }
        public DelegateCommand<VaultViewModel> DoubleClickVaultCommand { get; set; }
        public DelegateCommand LoginToVaultCommand { get; set; }

        public VaultConnectionViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, CR.Login_Vault_ModuleName)
        {
            this.regionManager = regionManager;

            SelectVaultCommand = new DelegateCommand<VaultViewModel>(ExecuteSelectVaultCommand);
            DoubleClickVaultCommand = new DelegateCommand<VaultViewModel>(ExecuteDoubleClickVaultCommand);
            LoginToVaultCommand = new DelegateCommand(ExecuteLoginToVault);

            Filter = new VaultFilterViewModel();
            Filter.Changed += FilterChanged;
        }

        private void ExecuteDoubleClickVaultCommand(VaultViewModel vaultVM)
        {
            ExecuteSelectVaultCommand(vaultVM);

            if (vaultVM.IsOnline)
            {
                ExecuteLoginToVault();
            }
        }

        private void ExecuteSelectVaultCommand(VaultViewModel vaultVM)
        {
            vaultVM.IsSelected = true;
            SelectedVault = vaultVM;
        }

        private void FilterChanged(object sender, Predicate<object> predicate)
        {
            Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Vaults.Filter = predicate;

                    if (sender is string propertyName)
                    {
                        switch (propertyName)
                        {
                            case nameof(Filter.Name):
                                Filter.IsNameFocused = true;
                                break;
                        }
                    }
                });
            });
        }

        private void ExecuteLoginToVault()
        {
            var serverProxy = unityContainer.Resolve<ServerProxy>();

            var vaultProxy = serverProxy.LoginToVault(SelectedVault.Guid);

            unityContainer.RegisterInstance(vaultProxy);

            EventOperator.GetEvent<ConnectedVaultChangedEvent>().Publish();

            var mainModuleNavParams = new MainModuleNavParams
            {
                IsAppBarVisible = false,
                AppBarTitle = "M-Files VB Script Explorer",
                AppBarSubTitle = null,
                InitModuleName = CR.VBScriptExplorer_ModuleName,
            };

            regionManager.RequestNavigate(CR.App_RegionHolderName, CR.Main_ModuleName, new NavigationParameters
            {
                { "p", mainModuleNavParams }
            });
        }

        private void VaultSelected(string vaultGuid)
        {
            foreach (var vault in vaultsSource)
            {
                if (vault.Guid != vaultGuid && vault.IsSelected)
                {
                    vault.IsSelected = false;
                }
                if (vault.Guid == vaultGuid && !vault.IsSelected)
                {
                    vault.IsSelected = true;
                }
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            vaultsSource = (List<VaultViewModel>)navigationContext.Parameters["vaults"];

            var vaultsView = CollectionViewSource.GetDefaultView(vaultsSource);
            vaultsView.SortDescriptions.Add(new SortDescription(nameof(VaultViewModel.Name), ListSortDirection.Ascending));

            Vaults = vaultsView;

            TriggerKeyScrollToTop = Guid.NewGuid().ToString();
        }
    }
}
