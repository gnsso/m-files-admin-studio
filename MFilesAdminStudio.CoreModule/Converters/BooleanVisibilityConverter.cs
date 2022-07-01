using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MFilesAdminStudio.CoreModule.Converters
{
    /// <summary>
    /// usage: Visibility="{Binding, Converter={x:Static converters:BooleanVisibilityConverter.Instance}, ConverterParameter=reverse}" />
    ///     value is boolean and true, param not contains reverse: Visible 
    ///     value is boolean and true, param contains reverse, param not contains hidden: Collapsed
    ///     value is boolean and true, param contains reverse and hidden: Hidden
    ///     value is boolean and false, param contains reverse: Visible 
    ///     value is boolean and false, param not contains reverse, param contains hidden: Hidden
    ///     value is boolean and false, param not contains reverse, param not contains hidden: Collapsed
    ///     value is not boolean: Visible 
    /// </summary>
    public class BooleanVisibilityConverter : IValueConverter
    {
        public static readonly BooleanVisibilityConverter Instance = new BooleanVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value?.ToString();
            var prm = parameter?.ToString();

            if (bool.TryParse(str, out var boolean))
            {
                if (boolean)
                {
                    if (prm?.Contains("reverse") ?? false)
                    {
                        return prm.Contains("hidden") ? Visibility.Hidden : Visibility.Collapsed;
                    }
                    else
                    {
                        return Visibility.Visible;
                    }
                }
                else
                {
                    if (prm?.Contains("reverse") ?? false)
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return prm?.Contains("hidden") ?? false ? Visibility.Hidden : Visibility.Collapsed;
                    }
                }
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
