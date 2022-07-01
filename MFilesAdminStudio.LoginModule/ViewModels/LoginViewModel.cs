using MFilesAdminStudio.CoreModule.Events;
using MFilesAdminStudio.CoreModule.Models;
using MFilesAdminStudio.CoreModule.ViewModels;
using MFilesAdminStudio.LoginModule.Events;
using MFilesAdminStudio.Services;
using MFilesAdminStudio.Services.License;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Resolution;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.LoginModule.ViewModels
{
    public class LoginViewModel : ModuleViewModelBase
    {
        public LoginViewModel(IUnityContainer unityContainer) : base(unityContainer, CR.Login_ModuleName)
        {

        }
    }
}
