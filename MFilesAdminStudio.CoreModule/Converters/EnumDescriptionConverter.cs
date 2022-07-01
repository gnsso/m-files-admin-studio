using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MFilesAdminStudio.CoreModule.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public static readonly EnumDescriptionConverter Instance = new EnumDescriptionConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum @enum)
            {
                var field = @enum.GetType().GetField(@enum.ToString());

                return field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
