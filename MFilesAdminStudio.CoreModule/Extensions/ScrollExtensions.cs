using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MFilesAdminStudio.CoreModule
{
    public static class ScrollExtensions
    {
        public static string GetTriggerScrollToTop(DependencyObject obj)
        {
            return (string)obj.GetValue(TriggerScrollToTopProperty);
        }

        public static void SetTriggerScrollToTop(DependencyObject obj, string value)
        {
            obj.SetValue(TriggerScrollToTopProperty, value);
        }

        public static readonly DependencyProperty TriggerScrollToTopProperty =
            DependencyProperty.RegisterAttached("TriggerScrollToTop", typeof(string), typeof(ScrollExtensions), new PropertyMetadata("", ScrollToTopPropertyChanged));

        private static void ScrollToTopPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToTop();
            }
        }
    }
}
