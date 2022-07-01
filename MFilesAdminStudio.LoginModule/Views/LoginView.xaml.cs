using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.LoginModule.Events;
using Prism.Ioc;
using Prism.Regions;
using System.Windows.Controls;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.LoginModule.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            Loaded += delegate
            {
                ContainerLocator.Container.Resolve<IRegionManager>().RequestNavigate(CR.Login_RegionHolderName, CR.Login_Server_ModuleName);
            };
        }
    }
}
