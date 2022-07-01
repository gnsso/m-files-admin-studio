using MFilesAdminStudio.Proxies.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MFilesAdminStudio.VBScriptExplorerModule.Converters
{
    public class VBScriptTypeIconBackgroundConverter : IValueConverter
    {
        public static VBScriptTypeIconBackgroundConverter Instance = new VBScriptTypeIconBackgroundConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is VBScriptType scriptType)
            {
                return Colors.Transparent;
            }

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
