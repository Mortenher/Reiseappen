using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Reiseappen2.HamburgerApp.Converters
{
    public class DollarsToNok : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((double)value * 8.53);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
