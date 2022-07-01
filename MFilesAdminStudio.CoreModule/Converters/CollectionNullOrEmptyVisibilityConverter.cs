using System;
using System.Collections;
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
    /// usage: Visibility="{Binding, Converter={x:Static converters:CollectionNullOrEmptyVisibilityConverter.Instance}, ConverterParameter=reverse:hidden}" />
    ///     value is boolean and true, param not contains reverse: Visible 
    ///     value is boolean and true, param contains reverse, param not contains hidden: Collapsed
    ///     value is boolean and true, param contains reverse and hidden: Hidden
    ///     value is boolean and false, param contains reverse: Visible 
    ///     value is boolean and false, param not contains reverse, param contains hidden: Hidden
    ///     value is boolean and false, param not contains reverse, param not contains hidden: Collapsed
    ///     value is not boolean: Visible 
    /// </summary>
    public class CollectionNullOrEmptyVisibilityConverter : IValueConverter
    {
        public static readonly CollectionNullOrEmptyVisibilityConverter Instance = new CollectionNullOrEmptyVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var prm = parameter?.ToString();

            if (value == null || !(value is IEnumerable val) || !val.GetEnumerator().MoveNext())
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
            else
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
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
