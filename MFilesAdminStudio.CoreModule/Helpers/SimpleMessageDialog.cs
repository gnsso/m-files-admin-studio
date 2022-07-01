using MaterialDesignThemes.Wpf;
using MFilesAdminStudio.CoreModule.ViewModels;
using MFilesAdminStudio.CoreModule.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MFilesAdminStudio.CoreModule.Helpers
{
    public static class SimpleMessageDialog
    {
        private static readonly SolidColorBrush closeRed;
        private static readonly SolidColorBrush primaryMid;
        private static readonly SolidColorBrush softBlack;
        private static readonly SolidColorBrush checkGreen;

        static SimpleMessageDialog()
        {
            if (closeRed == null)
            {
                closeRed = (SolidColorBrush)Application.Current.FindResource("CloseRed");
            }

            if (primaryMid == null)
            {
                primaryMid = (SolidColorBrush)Application.Current.FindResource("PrimaryHueMidBrush");
            }

            if (softBlack == null)
            {
                softBlack = (SolidColorBrush)Application.Current.FindResource("SoftBlack");
            }

            if (checkGreen == null)
            {
                checkGreen = (SolidColorBrush)Application.Current.FindResource("CheckGreen");
            }
        }

        public static async Task<bool> ConfirmNormal(string message, string confirm = "TAMAM", string cancel = "İPTAL", string dialog = "RootDialog")
        {
            return await Confirm(message, confirm, cancel, false, dialog);
        }

        public static async Task<bool> ConfirmWarning(string message, string confirm = "TAMAM", string cancel = "İPTAL", string dialog = "RootDialog")
        {
            return await Confirm(message, confirm, cancel, true, dialog);
        }

        private static async Task<bool> Confirm(string message, string confirm = "TAMAM", string cancel = "İPTAL", bool warning = false, string dialog = "RootDialog")
        {
            var messageView = new SimpleConfirmMessageView
            {
                DataContext = new SimpleConfirmMessageViewModel
                {
                    IconKind = warning ? PackIconKind.WarningCircle : PackIconKind.InfoCircle,
                    IconForeground = warning ? closeRed : primaryMid,
                    ConfirmCommandBackground = warning ? closeRed : primaryMid,
                    ConfirmCommandBorderBrush = warning ? closeRed : primaryMid,
                    ConfirmCommandText = confirm,
                    CancelCommandText = cancel,
                    Message = message
                }
            };

            return (bool)await DialogHost.Show(messageView, dialog);
        }

        public static async Task Error(string message, string dialog = "RootDialog")
        {
            var messageView = new SimpleInfoMessageView
            {
                DataContext = new SimpleInfoMessageViewModel
                {
                    IconKind = PackIconKind.Error,
                    IconForeground = closeRed,
                    OkCommandText = "TAMAM",
                    OkCommandForeground = softBlack,
                    Message = message,
                }
            };

            await DialogHost.Show(messageView, dialog);
        }

        public static async Task Ok(string message, string dialog = "RootDialog")
        {
            var messageView = new SimpleInfoMessageView
            {
                DataContext = new SimpleInfoMessageViewModel
                {
                    IconKind = PackIconKind.Check,
                    IconForeground = checkGreen,
                    OkCommandText = "TAMAM",
                    OkCommandForeground = softBlack,
                    Message = message,
                }
            };

            await DialogHost.Show(messageView, dialog);
        }
    }
}
