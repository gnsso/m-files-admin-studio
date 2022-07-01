using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MFilesAdminStudio.CoreModule.Helpers
{
    public static class ControlHelper
    {
        public static T TryFindParent<T>(DependencyObject current) where T : class
        {
            var parent = VisualTreeHelper.GetParent(current);

            if (parent == null) return null;

            if (parent is T) return parent as T;
            else return TryFindParent<T>(parent);
        }
    }
}
