using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    /// <summary>
    /// Command to generate a pallet of colors
    /// Can always executed
    /// Generates a list of colors from 255, 0, 0 to 0,0,255
    /// </summary>
    public class GenerateColorPalletCommand : ICommand
    {
        /// <summary>
        /// Not in use
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        /// <summary>
        /// Always true
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>always true</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Execute method
        /// Generates a color pallet
        /// </summary>
        /// <param name="parameter">Not in use</param>
        public void Execute(object parameter)
        {
            List<Color> colors = new List<Color>();

            int x = 255 * 3;
            int y = x / OwnDataContext.Instance.CountOfColors;

            int r = 255, g = 0, b = 0;
            // Add RED as the first color
            colors.Add(Color.FromArgb(r, g, b));
            for (int i = 0; i < OwnDataContext.Instance.CountOfColors; i++)
            {
                // Decrease the byte of red by y and and increase the byte of green also by y 
                if (r > 0 && g <= 255 && b == 0)
                {
                    r = r - y;
                    g = g + y;
                    // Add the calculated color to the list of colors
                    colors.Add(Color.FromArgb(r, g, b));
                    if (r - y < 0)
                    {
                        r = 0;
                        g = 255;
                        // Add green to the list of colors
                        colors.Add(Color.FromArgb(r, g, b));
                    }
                }
                // Decrease the byte of green by y and and increase the byte of blue also by y 
                else if (g > 0 && b <= 255 && r == 0)
                {
                    g = g - y;
                    b = b + y;
                    // Add the calculated color to the list of colors
                    colors.Add(Color.FromArgb(r, g, b));
                    if (g - y < 0)
                    {
                        g = 0;
                        b = 255;
                        // Add blue to the list of colors
                        colors.Add(Color.FromArgb(r, g, b));
                    }
                }
                // Decrease the byte of blue by y and and increase the byte of red also by y 
                else if (b > 0 && r <= 255 && g == 0)
                {
                    b = b - y;
                    r = r + y;
                    // Add the calculated color to the list of colors
                    colors.Add(Color.FromArgb(r, g, b));
                    if (b - y < 0)
                    {
                        b = 0;
                        r = 255;
                        // Reduce list of colors to distinct colors
                        OwnDataContext.Instance.Colors = new List<Color>(colors.Distinct());
                    }
                }
            }
            // Reduce list of colors to the selected count of colors
            while (OwnDataContext.Instance.Colors.Count > OwnDataContext.Instance.CountOfColors)
            {
                OwnDataContext.Instance.Colors.RemoveAt(OwnDataContext.Instance.CountOfColors - 1);
            }
            // Trigger PropertyChanged event for Colors
            OwnDataContext.Instance.OnPropertyChanged("Colors");
        }
    }
}
