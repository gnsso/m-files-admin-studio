using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MFilesAdminStudio.CoreModule.ViewModels
{
    public class ListVaultsItemViewModel : BindableBase
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        private string guid;
        public string Guid
        {
            get { return guid; }
            set { SetProperty(ref guid, value); }
        }

        private bool isOnline;
        public bool IsOnline
        {
            get { return isOnline; }
            set { SetProperty(ref isOnline, value); }
        }

        private bool isCurrentVault;
        public bool IsCurrentVault
        {
            get { return isCurrentVault; }
            set { SetProperty(ref isCurrentVault, value); }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }
    }
}
