using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    public class GenerateColorPalletCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            List<Color> colors = new List<Color>();

            int x = 255 * 3;
            int y = x / OwnDataContext.Instance.CountOfColors;

            int r = 255, g = 0, b = 0;
            colors.Add(Color.FromArgb(r, g, b));
            for (int i = 0; i < OwnDataContext.Instance.CountOfColors; i++)
            {
                if (r > 0 && g <= 255 && b == 0)
                {
                    r = r - y;
                    g = g + y;
                    colors.Add(Color.FromArgb(r, g, b));
                    if (r - y < 0)
                    {
                        r = 0;
                        g = 255;
                        colors.Add(Color.FromArgb(r, g, b));
                    }
                }
                else if (g > 0 && b <= 255 && r == 0)
                {
                    g = g - y;
                    b = b + y;
                    colors.Add(Color.FromArgb(r, g, b));
                    if (g - y < 0)
                    {
                        g = 0;
                        b = 255;
                        colors.Add(Color.FromArgb(r, g, b));
                    }
                }
                else if (b > 0 && r <= 255 && g == 0)
                {
                    b = b - y;
                    r = r + y;
                    colors.Add(Color.FromArgb(r, g, b));
                    if (b - y < 0)
                    {
                        b = 0;
                        r = 255;
                        OwnDataContext.Instance.Colors = new List<Color>(colors.Distinct());
                    }
                }
            }
            while (OwnDataContext.Instance.Colors.Count > OwnDataContext.Instance.CountOfColors)
            {
                OwnDataContext.Instance.Colors.RemoveAt(OwnDataContext.Instance.CountOfColors - 1);
            }
            OwnDataContext.Instance.OnPropertyChanged("Colors");
        }
    }
}
