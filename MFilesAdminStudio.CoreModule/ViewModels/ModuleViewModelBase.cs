using MFilesAdminStudio.Services;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Resolution;
using Resources = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.CoreModule.ViewModels
{
    public class ModuleViewModelBase : BindableBase, INavigationAware
    {
        protected readonly string moduleName;
        protected readonly IUnityContainer unityContainer;
        protected readonly ILoggingService logger;

        public ModuleViewModelBase(IUnityContainer unityContainer, string moduleName)
        {
            this.unityContainer = unityContainer;
            this.moduleName = moduleName;

            logger = unityContainer.Resolve<ILoggingService>(new ParameterOverride("module", moduleName));
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
            OnNavigatedFrom(navigationContext);

            logger.Info(string.Format(
                Resources.Global_Message_NavigatedFromModule, 
                moduleName, 
                navigationContext.Parameters.Count == 0 ? "no params" : string.Join(", ", navigationContext.Parameters)));
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            OnNavigatedTo(navigationContext);

            logger.Info(string.Format(
                Resources.Global_Message_NavigatedToModule,
                moduleName,
                navigationContext.Parameters.Count == 0 ? "no params" : string.Join(", ", navigationContext.Parameters)));
        }
    }
}
