using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    public class SaveSettingsCommand : ICommand
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
            config.TuyaAccessSecret = OwnDataContext.Instance.TuyaSecret;
            config.TuyaAccessId = OwnDataContext.Instance.TuyaAccessId;
            config.Colors = OwnDataContext.Instance.Colors.ToList();
            config.Devices = OwnDataContext.Instance.CloudLights.ToList();

            config.UseTuyaCloud = OwnDataContext.Instance.UseTuyaCloud;
            config.TuyaRegion = OwnDataContext.Instance.TuyaRegion;
            config.TuyaAnyDevice = OwnDataContext.Instance.TuyaAnyDevice;
            config.Delay = OwnDataContext.Instance.Delay;
            config.SelectedLightIds.Clear();
            foreach (var selLight in OwnDataContext.Instance.SelectedLights)
            {
                config.SelectedLightIds.Add(selLight.DeviceId);
            }
            config.SoundDevice = OwnDataContext.Instance.SelectedDevice.Key;
            Configuration.Save();
        }
    }
}
