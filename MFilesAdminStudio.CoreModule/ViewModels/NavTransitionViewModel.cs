using MFilesAdminStudio.CoreModule.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;
using static MFilesAdminStudio.CoreModule.Helpers.StaticFunctions;
using System.Windows.Media;
using MFilesAdminStudio.CoreModule.Controls;

namespace MFilesAdminStudio.CoreModule.ViewModels
{
    public class NavTransitionViewModel : ModuleViewModelBase
    {
        private readonly IRegionManager regionManager;

        private NavTransitionPayload payload;

        private object view;
        public object View
        {
            get { return view; }
            set { SetProperty(ref view, value); }
        }

        public NavTransitionViewModel(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, CR.Global_NavTransitionModuleName) 
        {
            this.regionManager = regionManager;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            View = null;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Require(navigationContext?.Parameters != null, "Navigation context or params was null");
            Require(navigationContext.Parameters.TryGetValue("payload", out payload) && payload != null, "Navigation payload must be set");
            Require(payload.CurrentView != null, "Transition view must be set");
            Require(payload.Function != null, "Transition function must be set");

            View = payload.CurrentView;

            Task.Run(() => payload.Function(Application.Current.Dispatcher)).ContinueWith(task => 
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        regionManager.RequestNavigate(payload.ToRegion, payload.ToView, new NavigationParameters { { task.Result.key, task.Result.value } });
                    }
                    else
                    {
                        View = new ErrorStack
                        {
                            Text = task.Exception?.InnerException?.Message ?? "An unhandled error occured",
                            GoBackCommand = new DelegateCommand(() => regionManager.Regions[payload.FromRegion].NavigationService.Journal.GoBack())
                        };
                    }
                });
            });
        }
    }
}
