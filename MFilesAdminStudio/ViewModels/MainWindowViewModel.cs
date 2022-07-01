using MaterialDesignThemes.Wpf;
using MFilesAdminStudio.CoreModule;
using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.CoreModule.ViewModels;
using MFilesAdminStudio.CoreModule.Views;
using MFilesAdminStudio.Proxies;
using MFilesAdminStudio.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;
using Unity;
using Unity.Resolution;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IUnityContainer container;
        private readonly ILoggingService logger;

        private string title = CR.Global_ShortTitle;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public double InitialWidth { get; } = double.Parse(CR.App_InitialWidth);
        public double InitialHeight { get; } = double.Parse(CR.App_InitialHeight);
        public ResizeMode InitialResizeMode { get; set; } = ResizeMode.NoResize;

        private Visibility appBarButtonsVisibility = Visibility.Collapsed;
        public Visibility AppBarButtonsVisibility
        {
            get { return appBarButtonsVisibility; }
            set { SetProperty(ref appBarButtonsVisibility, value); }
        }

        private string connectedVaultName;
        public string ConnectedVaultName
        {
            get { return connectedVaultName; }
            set { SetProperty(ref connectedVaultName, value); }
        }

        public DelegateCommand ChangeVaultCommand { get; set; }
        public DelegateCommand OpenAboutCommand { get; set; }

        public MainWindowViewModel(IUnityContainer container)
        {
            this.container = container;

            logger = container.Resolve<ILoggingService>(new ParameterOverride("module", CR.App_ModuleName));

            ChangeVaultCommand = new DelegateCommand(ExecuteChangeVault);
            OpenAboutCommand = new DelegateCommand(ExecuteOpenAbout);

            var appSession = container.Resolve<AppSession>();

            EventOperator.GetEvent<AppTitleChangedEvent>().Subscribe(newTitle => Title = $"{newTitle}{(appSession.IsPreRelease ? " (pre-release)" : "")}");
            EventOperator.GetEvent<ConnectedVaultChangedEvent>().Subscribe(() =>
            {
                var serverProxy = this.container.Resolve<ServerProxy>();
                var vaultProxy = this.container.Resolve<VaultProxy>();

                ConnectedVaultName = $"{vaultProxy.Name} ({serverProxy.NetworkAddress})";
            });

            logger.Info("App initialized");
        }

        private void ExecuteChangeVault()
        {
            var vm = new ListVaultsViewModel(container);

            var view = new ListVaultsView
            {
                DataContext = vm
            };

            DialogHost.Show(view, CR.Main_RootDialogIdentifier, openedEventHandler: delegate { vm.Load(); });
        }

        private void ExecuteOpenAbout()
        {
            var appSession = container.Resolve<AppSession>();

            var view = new AboutView
            {
                DataContext = new AboutViewModel
                {
                    Description = "M-Files VB Script Explorer",
                    Version = $"v{appSession.FileVersion}{(appSession.IsPreRelease ? " (pre-release)" : "")}",
                }
            };

            DialogHost.Show(view, CR.Main_RootDialogIdentifier);
        }
    }
}
