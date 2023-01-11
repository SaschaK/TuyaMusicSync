using System;
using System.Globalization;
using System.Windows.Data;

namespace TuyaCoreController.Converter
{
    /// <summary>
    /// Converts the Color Text value to a System.Drawing.Color value
    /// </summary>
    internal class ColorNameConverter : IValueConverter
    {
        /// <summary>
        /// Convert the color text value to an useable System.Drawing.Color value
        /// </summary>
        /// <param name="value">Text value</param>
        /// <param name="targetType">Not in use</param>
        /// <param name="parameter">Not in use</param>
        /// <param name="culture">Not in use</param>
        /// <returns>Syste.Drawing.Color value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int a, r, g, b;
            a = System.Convert.ToInt32(value.ToString().Substring(0, 2), 16);
            r = System.Convert.ToInt32(value.ToString().Substring(2, 2), 16);
            g = System.Convert.ToInt32(value.ToString().Substring(4, 2), 16);
            b = System.Convert.ToInt32(value.ToString().Substring(6, 2), 16);
            var color = System.Drawing.Color.FromArgb(a, r, g, b);
            return color;
        }

        /// <summary>
        /// ConvertBack method - Returns always null
        /// </summary>
        /// <param name="value">Not in use</param>
        /// <param name="targetType">Not in use</param>
        /// <param name="parameter">Not in use</param>
        /// <param name="culture">Not in use</param>
        /// <returns>always null</returns>
        /// <exception cref="NullReferenceException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw null;
        }
    }
}
