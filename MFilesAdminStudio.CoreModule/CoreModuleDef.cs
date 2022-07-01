using MFilesAdminStudio.CoreModule.ViewModels;
using MFilesAdminStudio.CoreModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.CoreModule
{
    public class CoreModuleDef : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavTransitionView, NavTransitionViewModel>(CR.Global_NavTransitionModuleName);
        }
    }
}
