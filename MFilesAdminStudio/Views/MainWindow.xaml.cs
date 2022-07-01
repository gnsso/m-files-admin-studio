using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using MFilesAdminStudio.CoreModule;
using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.CoreModule.ViewModels;
using MFilesAdminStudio.CoreModule.Views;
using MFilesAdminStudio.Services;
using MFilesAdminStudio.ViewModels;
using Prism.Ioc;
using Squirrel;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.Views
{
    public partial class MainWindow : MaterialWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            EventOperator.GetEvent<MainModuleStartedEvent>().Subscribe(MainModuleStarted);

            Loaded += delegate 
            { 
                CheckForUpdates();
            };
        }

        private void MainModuleStarted(string mainAppBarTitle)
        {
            var appSession = ContainerLocator.Container.Resolve<AppSession>();

            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Application.Current.MainWindow.Title = $"{mainAppBarTitle}{(appSession.IsPreRelease ? " (pre-release)" : "")}";

            MinHeight = double.Parse(CR.App_MainMinHeight);
            MinWidth = double.Parse(CR.App_MainMinWidth);

            Application.Current.MainWindow.ResizeMode = ResizeMode.CanResize;

            if (DataContext is MainWindowViewModel vm)
            {
                vm.AppBarButtonsVisibility = Visibility.Visible;
            }
        }

        private void GoToAppPage(object sender, RoutedEventArgs e)
        {
            Process.Start(CR.App_GithubAppPageUrl);
        }

        private async void CheckForUpdates()
        {
            try
            {
                using (var mgr = await UpdateManager.GitHubUpdateManager(CR.App_GithubAppPageUrl))
                {
                    var release = await mgr.UpdateApp();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var logger = ContainerLocator.Container.Resolve<ILoggingService>((typeof(string), CR.App_ModuleName));

                    logger.Error($"[CheckForUpdates] {ex.Message}");
                }
                catch
                {

                }
            }
        }
    }
}
