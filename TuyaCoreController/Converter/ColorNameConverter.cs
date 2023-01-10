using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TuyaCoreController.Converter
{
    internal class ColorNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int a, r, g, b;
            a = System.Convert.ToInt32(value.ToString().Substring(0, 2), 16);
            r = System.Convert.ToInt32(value.ToString().Substring(2, 2), 16);
            g = System.Convert.ToInt32(value.ToString().Substring(4, 2), 16);
            b = System.Convert.ToInt32(value.ToString().Substring(6, 2), 16);
            var color = System.Drawing.Color.FromArgb(a, r, g, b);
            return color;//String.Format("R = {0}, G = {1}, B = {2}", color.R, color.G, color.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw null;
        }
    }
}
