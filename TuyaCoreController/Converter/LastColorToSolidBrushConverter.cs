using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TuyaCoreController.Converter
{
    /// <summary>
    /// Converts System.Drawing.Color to a SolidBrush value
    /// </summary>
    internal class LastColorToSolidBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts System.Drawing.Color to a SolidBrush value for the useage in the gui (display lights LastColor property)
        /// </summary>
        /// <param name="value">LastColor (System.Drawing.Color)</param>
        /// <param name="targetType">SolidBrush</param>
        /// <param name="parameter">Not in use</param>
        /// <param name="culture">Not in use</param>
        /// <returns>SolidBrush value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Drawing.Color color = (System.Drawing.Color)value;
            Brush resBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            System.Windows.Media.Color targetColor = Color.FromRgb(color.R, color.G, color.B);
            resBrush = new SolidColorBrush(targetColor);

            return resBrush;
        }

        /// <summary>
        /// ConvertBack method - Returns always null
        /// </summary>
        /// <param name="value">Not in use</param>
        /// <param name="targetType">Not in use</param>
        /// <param name="parameter">Not in use</param>
        /// <param name="culture">Not in use</param>
        /// <returns>always null</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
