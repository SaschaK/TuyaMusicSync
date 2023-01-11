using System;
using System.Globalization;
using System.Windows.Data;

namespace TuyaCoreController.Converter
{
    /// <summary>
    /// Converts the Audio spectrum byte value to an integer value
    /// Used by the Audio-Input progressbars to show the audio spectrum
    /// </summary>
    internal class ByteToIntConverter : IValueConverter
    {
        /// <summary>
        /// Converts the byte value to an integer value
        /// </summary>
        /// <param name="value">byte value</param>
        /// <param name="targetType">Integer</param>
        /// <param name="parameter">Not in use</param>
        /// <param name="culture">Not in use</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte result = new byte();
            if (value != null)
            {
                result = (byte)value;
            }
            return System.Convert.ToInt32(result);
        }

        /// <summary>
        /// ConvertBack method - Returns always 0
        /// </summary>
        /// <param name="value">Not in use</param>
        /// <param name="targetType">Not in use</param>
        /// <param name="parameter">Not in use</param>
        /// <param name="culture">Not in use</param>
        /// <returns>always 0</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
