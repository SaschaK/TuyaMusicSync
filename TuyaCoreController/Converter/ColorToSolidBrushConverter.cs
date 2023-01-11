using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TuyaCoreController.Converter
{
    /// <summary>
    /// Converts color value to a SolidBrush value
    /// </summary>
    internal class ColorToSolidBrushConverter : IValueConverter
    {
        /// <summary>
        /// Uses a color value to create a SolidBrush value to show the current light color on the gui
        /// </summary>
        /// <param name="value">Color value</param>
        /// <param name="targetType">SolidBrush</param>
        /// <param name="parameter">Not in use</param>
        /// <param name="culture">Not in use</param>
        /// <returns>SolidBrush value</returns>
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

        /// <summary>
        /// ConvertBack method - Always returns null
        /// </summary>
        /// <param name="value">Not in use</param>
        /// <param name="targetType">Not in use</param>
        /// <param name="parameter">Not in use</param>
        /// <param name="culture">Not in use</param>
        /// <returns>Always null</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
