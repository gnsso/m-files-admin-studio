using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CR = MFilesAdminStudio.CoreModule.Properties.Resources;

namespace MFilesAdminStudio.CoreModule.ViewModels
{
    public class AboutViewModel : BindableBase
    {
        private string description;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        private string version;
        public string Version
        {
            get { return version; }
            set { SetProperty(ref version, value); }
        }

        private string subString;
        public string SubString
        {
            get { return subString; }
            set { SetProperty(ref subString, value); }
        }

        public DelegateCommand OpenGithubCommand { get; set; }

        public AboutViewModel()
        {
            OpenGithubCommand = new DelegateCommand(ExecuteOpenGithub);
        }

        private void ExecuteOpenGithub()
        {
            Process.Start(CR.App_GithubAppPageUrl);
        }
    }
}
