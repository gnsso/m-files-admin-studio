using MaterialDesignThemes.Wpf;
using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.Proxies;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Unity;

namespace MFilesAdminStudio.CoreModule.ViewModels
{
    public class ListVaultsViewModel : BindableBase
    {
        private readonly IUnityContainer unityContainer;

        private string currentVaultGuid;
        private List<ListVaultsItemViewModel> vaultsSource;

        private ICollectionView vaults;
        public ICollectionView Vaults
        {
            get { return vaults; }
            set { SetProperty(ref vaults, value); }
        }

        private ListVaultsItemViewModel selectedVault;
        public ListVaultsItemViewModel SelectedVault
        {
            get { return selectedVault; }
            set
            {
                SetProperty(ref selectedVault, value);

                if (value != null)
                {
                    VaultSelected(value.Guid);
                }

                IsSelectionEnabled = value != null && value.IsOnline && !value.IsCurrentVault;
            }
        }

        private bool isSelectionEnabled = false;
        public bool IsSelectionEnabled
        {
            get { return isSelectionEnabled; }
            set { SetProperty(ref isSelectionEnabled, value); }
        }

        private bool isLoadingVaults;
        public bool IsLoadingVaults
        {
            get { return isLoadingVaults; }
            set { SetProperty(ref isLoadingVaults, value); }
        }

        public DelegateCommand<ListVaultsItemViewModel> SelectVaultCommand { get; set; }
        public DelegateCommand<ListVaultsItemViewModel> DoubleClickVaultCommand { get; set; }
        public DelegateCommand FinishSelectionCommand { get; set; }

        public ListVaultsViewModel(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;

            SelectVaultCommand = new DelegateCommand<ListVaultsItemViewModel>(ExecuteSelectVaultCommand);
            DoubleClickVaultCommand = new DelegateCommand<ListVaultsItemViewModel>(ExecuteDoubleClickVaultCommand);
            FinishSelectionCommand = new DelegateCommand(ExecuteFinishSelectionCommand);
        }

        private void ExecuteDoubleClickVaultCommand(ListVaultsItemViewModel vaultVM)
        {
            ExecuteSelectVaultCommand(vaultVM);

            if (vaultVM.IsOnline && !vaultVM.IsCurrentVault)
            {
                ExecuteFinishSelectionCommand();
            }
        }

        private void ExecuteSelectVaultCommand(ListVaultsItemViewModel vaultVM)
        {
            vaultVM.IsSelected = true;
            SelectedVault = vaultVM;
        }

        private void ExecuteFinishSelectionCommand()
        {
            var serverProxy = unityContainer.Resolve<ServerProxy>();

            var vaultProxy = serverProxy.LoginToVault(SelectedVault.Guid);

            unityContainer.RegisterInstance(vaultProxy);
            EventOperator.GetEvent<ConnectedVaultChangedEvent>().Publish();

            DialogHost.CloseDialogCommand.Execute(true, null);
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

        public void Load()
        {
            IsLoadingVaults = true;

            Task.Delay(100).ContinueWith(t =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var serverProxy = unityContainer.Resolve<ServerProxy>();
                    var vaultProxy = unityContainer.Resolve<VaultProxy>();

                    currentVaultGuid = vaultProxy.Guid;

                    var serverVaults = serverProxy.GetVaults();

                    vaultsSource = serverVaults.Select(s => new ListVaultsItemViewModel
                    {
                        Name = s.name,
                        Guid = s.guid,
                        IsOnline = s.online,
                        IsCurrentVault = s.guid == currentVaultGuid,
                        IsSelected = false
                    }).ToList();

                    var vaultsView = CollectionViewSource.GetDefaultView(vaultsSource);
                    vaultsView.SortDescriptions.Add(new SortDescription(nameof(ListVaultsItemViewModel.Name), ListSortDirection.Ascending));

                    Vaults = vaultsView;

                    IsLoadingVaults = false;
                });
            });
        }
    }
}
