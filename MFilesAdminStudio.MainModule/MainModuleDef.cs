using MFilesAdminStudio.VBScriptExplorerModule.ViewModels;
using MFilesAdminStudio.VBScriptExplorerModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.MainModule
{
    public class MainModuleDef : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<VBScriptExplorerView, VBScriptExplorerViewModel>(CR.VBScriptExplorer_ModuleName);
        }
    }
}
