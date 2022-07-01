using MaterialDesignExtensions.Model;
using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.CoreModule.Models;
using MFilesAdminStudio.CoreModule.ViewModels;
using MFilesAdminStudio.MainModule.Models;
using MFilesAdminStudio.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Unity;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.MainModule.ViewModels
{
    public class MainViewModel : ModuleViewModelBase
    {
        private readonly IRepositoryService repository;
        private readonly IRegionManager regionManager;

        private readonly List<string> navigationJournal = new List<string>();
        private int navigationJournalCurrentIndex = -1;

        private bool isAppBarVisible;
        public bool IsAppBarVisible
        {
            get { return isAppBarVisible; }
            set { SetProperty(ref isAppBarVisible, value); }
        }

        private string appBarTitle;
        public string AppBarTitle
        {
            get { return appBarTitle; }
            set { SetProperty(ref appBarTitle, value); }
        }

        private string appBarSubTitle;
        public string AppBarSubTitle
        {
            get { return appBarSubTitle; }
            set { SetProperty(ref appBarSubTitle, value); }
        }

        private INavigationItem selectedNavItem;
        public INavigationItem SelectedNavItem
        {
            get { return selectedNavItem; }
            set
            {
                SetProperty(ref selectedNavItem, value);

                value.IsSelected = true;

                NavItems.OfType<ModuleNavigationItem>().Except(new[] { (ModuleNavigationItem)value }).ToList().ForEach(f => f.IsSelected = false);
            }
        }

        private List<INavigationItem> navItems;
        public List<INavigationItem> NavItems
        {
            get { return navItems; }
            set { SetProperty(ref navItems, value); }
        }

        private bool isNavBarButtonVisible;
        public bool IsNavBarButtonVisible
        {
            get { return isNavBarButtonVisible; }
            set { SetProperty(ref isNavBarButtonVisible, value); }
        }

        private bool isNavbarChecked = false;
        public bool IsNavbarChecked
        {
            get { return isNavbarChecked; }
            set { SetProperty(ref isNavbarChecked, value); }
        }

        public DelegateCommand<INavigationItem> NavItemSelectedCommand { get; set; }

        public MainViewModel(IUnityContainer unityContainer, IRegionManager regionManager, IRepositoryService repository) : base(unityContainer, CR.Main_ModuleName)
        {
            this.repository = repository;
            this.regionManager = regionManager;

            NavItemSelectedCommand = new DelegateCommand<INavigationItem>(item => ExecuteNavItemSelected(((ModuleNavigationItem)item).ModuleName, NavigationItemSelectionSource.NavigationPanel));

            EventOperator.GetEvent<NavigationCardClickedEvent>().Subscribe(module => ExecuteNavItemSelected(module, NavigationItemSelectionSource.NavigationCardOrDynamic), true);
            EventOperator.GetEvent<GoBackRequestedEvent>().Subscribe(() => ExecuteNavItemSelected(null, NavigationItemSelectionSource.GoBackButton));
            EventOperator.GetEvent<GoForwardRequestedEvent>().Subscribe(() => ExecuteNavItemSelected(null, NavigationItemSelectionSource.GoForwardButton));
        }

        private void ExecuteNavItemSelected(string module, NavigationItemSelectionSource source)
        {
            logger.Info($"Selected module: {module}, source: {source}");

            if (source == NavigationItemSelectionSource.GoBackButton)
            {
                navigationJournalCurrentIndex--;
            }
            else if (source == NavigationItemSelectionSource.GoForwardButton)
            {
                navigationJournalCurrentIndex++;
            }
            else
            {
                if (module == null)
                {
                    return;
                }
                else if (navigationJournal.Count > 0 && module == navigationJournal[navigationJournalCurrentIndex])
                {
                    return;
                }

                navigationJournal.Add(module);
                navigationJournalCurrentIndex++;

                if (navigationJournal.Count > navigationJournalCurrentIndex + 1)
                {
                    navigationJournal.RemoveRange(navigationJournalCurrentIndex, navigationJournal.Count - navigationJournalCurrentIndex - 1);
                }
            }

            EventOperator.GetEvent<NavigationJournalChangedEvent>().Publish(new NavigationJournalParams
            {
                CanGoBack = navigationJournalCurrentIndex > 0,
                IsGoBackVisible = true,
                CanGoForward = navigationJournalCurrentIndex < navigationJournal.Count - 1,
                IsGoForwardVisible = true
            });

            module = navigationJournal[navigationJournalCurrentIndex];

            var currentSelectedItem = NavItems.OfType<ModuleNavigationItem>().Single(s => s.ModuleName == module);

            SelectedNavItem = currentSelectedItem;

            IsNavbarChecked = false;

            regionManager.RequestNavigate(CR.Main_RegionHolderName, module);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var navParams = navigationContext.Parameters["p"] as MainModuleNavParams;

            IsAppBarVisible = navParams.IsAppBarVisible;
            IsNavBarButtonVisible = navParams.IsNavBarButtonVisible;
            AppBarTitle = navParams.AppBarTitle;
            AppBarSubTitle = navParams.AppBarSubTitle;
            NavItems = navParams.NavItems;

            EventOperator.GetEvent<MainModuleStartedEvent>().Publish(navParams.AppBarTitle);

            regionManager.RequestNavigate(CR.Main_RegionHolderName, navParams.InitModuleName, new NavigationParameters
            {
                { navParams.InitModuleParam.key, navParams.InitModuleParam.value }
            });
        }
    }
}
