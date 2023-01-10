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
    internal class LastColorToSolidBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Drawing.Color color = (System.Drawing.Color)value;
            Brush resBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            System.Windows.Media.Color targetColor = Color.FromRgb(color.R, color.G, color.B);
            resBrush = new SolidColorBrush(targetColor);

            return resBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
