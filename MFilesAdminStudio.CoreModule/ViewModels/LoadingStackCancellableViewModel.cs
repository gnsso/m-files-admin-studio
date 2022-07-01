using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.CoreModule.ViewModels
{
    public class LoadingStackCancellableViewModel : BindableBase
    {
        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public DelegateCommand CancelCommand { get; }

        public event EventHandler Cancelled;

        public LoadingStackCancellableViewModel()
        {
            CancelCommand = new DelegateCommand(() => 
            {
                DialogHost.CloseDialogCommand.Execute(null, null);
                Cancelled?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
