using MFilesAdminStudio.LoginModule.ViewModels;
using MFilesAdminStudio.LoginModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Linq;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.LoginModule
{
    public class LoginModuleDef : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(CR.App_RegionHolderName, typeof(LoginView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ServerConnectionView, ServerConnectionViewModel>(CR.Login_Server_ModuleName);
            containerRegistry.RegisterForNavigation<VaultConnectionView, VaultConnectionViewModel>(CR.Login_Vault_ModuleName);
        }
    }
}
