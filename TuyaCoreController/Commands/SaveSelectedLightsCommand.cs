using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    public class SaveSelectedLightsCommand : ICommand
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
            var config = Configuration.Instance;
            config.SelectedLightIds.Clear();
            foreach (var selLight in OwnDataContext.Instance.SelectedLights)
            {
                config.SelectedLightIds.Add(selLight.DeviceId);
            }
            Configuration.Save();
        }
    }
}
