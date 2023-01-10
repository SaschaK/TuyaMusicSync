using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    public class SelectLightCommand : ICommand
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
            var dev = OwnDataContext.Instance.CloudLights.FirstOrDefault(a=>a.DeviceId== parameter.ToString());
            if (dev!=null)
            {
                OwnDataContext.Instance.SelectedLights.Add(dev);
            }
        }
    }
}
