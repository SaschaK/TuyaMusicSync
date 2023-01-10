using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace TuyaCoreController.Converter
{
    internal class ColorToSolidBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var valStr = value.ToString();
            byte a, r, g, b;
            a = System.Convert.ToByte(valStr.Substring(0, 2), 16);
            r = System.Convert.ToByte(valStr.Substring(2, 2), 16);
            g = System.Convert.ToByte(valStr.Substring(4, 2), 16);
            b = System.Convert.ToByte(valStr.Substring(6, 2), 16);
            var color = System.Windows.Media.Color.FromArgb(a, r, g, b);
            
            Brush resBrush = new SolidColorBrush(color);
            return resBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
