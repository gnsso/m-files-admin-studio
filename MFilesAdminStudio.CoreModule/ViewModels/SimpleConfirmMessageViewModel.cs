using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace MFilesAdminStudio.CoreModule.ViewModels
{
    public class SimpleConfirmMessageViewModel : BindableBase
    {
        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private string confirmCommandText;

        public string ConfirmCommandText
        {
            get { return confirmCommandText; }
            set { SetProperty(ref confirmCommandText, value); }
        }

        private string cancelCommandText;
        public string CancelCommandText
        {
            get { return cancelCommandText; }
            set { SetProperty(ref cancelCommandText, value); }
        }

        private PackIconKind iconKind;
        public PackIconKind IconKind
        {
            get { return iconKind; }
            set { SetProperty(ref iconKind, value); }
        }

        private SolidColorBrush iconForeground;
        public SolidColorBrush IconForeground
        {
            get { return iconForeground; }
            set { SetProperty(ref iconForeground, value); }
        }

        private SolidColorBrush confirmCommandBorderBrush;
        public SolidColorBrush ConfirmCommandBorderBrush
        {
            get { return confirmCommandBorderBrush; }
            set { SetProperty(ref confirmCommandBorderBrush, value); }
        }

        private SolidColorBrush confirmCommandBackground;
        public SolidColorBrush ConfirmCommandBackground
        {
            get { return confirmCommandBackground; }
            set { SetProperty(ref confirmCommandBackground, value); }
        }

        public DelegateCommand ConfirmCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public SimpleConfirmMessageViewModel()
        {
            ConfirmCommand = new DelegateCommand(ExecuteConfirm);
            CancelCommand = new DelegateCommand(ExecuteCancel);
        }

        private void ExecuteConfirm()
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        private void ExecuteCancel()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }
    }
}
