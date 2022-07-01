using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace MFilesAdminStudio.CoreModule.ViewModels
{
    public class SimpleInfoMessageViewModel : BindableBase
    {
        private string message;
        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        private string okCommandText;

        public string OkCommandText
        {
            get { return okCommandText; }
            set { SetProperty(ref okCommandText, value); }
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

        private SolidColorBrush okCommandBorderBrush;
        public SolidColorBrush OkCommandBorderBrush
        {
            get { return okCommandBorderBrush; }
            set { SetProperty(ref okCommandBorderBrush, value); }
        }

        private SolidColorBrush okCommandBackground;
        public SolidColorBrush OkCommandBackground
        {
            get { return okCommandBackground; }
            set { SetProperty(ref okCommandBackground, value); }
        }

        private SolidColorBrush okCommandForeground;
        public SolidColorBrush OkCommandForeground
        {
            get { return okCommandForeground; }
            set { SetProperty(ref okCommandForeground, value); }
        }

        public DelegateCommand OkCommand { get; set; }

        public SimpleInfoMessageViewModel()
        {
            OkCommand = new DelegateCommand(ExecuteOk);
        }

        private void ExecuteOk()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }
    }
}
