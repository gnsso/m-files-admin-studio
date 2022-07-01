using MFilesAdminStudio.CoreModule.Models;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.CoreModule
{
    public static class RegionManagerExtensions
    {
        public static void RequestTransitionNavigate(
            this IRegionManager regionManager,
            string region,
            string fromView,
            string toView,
            object currentView,
            Func<Dispatcher, (string key, object value)> function)
        {
            RequestTransitionNavigate(regionManager, region, region, fromView, toView, currentView, function);
        }

        public static void RequestTransitionNavigate(
            this IRegionManager regionManager,
            string fromRegion,
            string toRegion,
            string fromView,
            string toView,
            object currentView,
            Func<Dispatcher, (string key, object value)> function)
        {
            var payload = new NavTransitionPayload
            {
                FromRegion = fromRegion,
                ToRegion = toRegion,
                FromView = fromView,
                ToView = toView,
                CurrentView = currentView,
                Function = function
            };

            regionManager.RequestNavigate(fromRegion, CR.Global_NavTransitionModuleName, new NavigationParameters
            {
                { CR.Global_NavTransitionPayloadKey, payload }
            });
        }
    }
}
