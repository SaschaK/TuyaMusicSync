using System;
using System.Linq;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    /// <summary>
    /// Save settings command
    /// </summary>
    public class SaveSettingsCommand : ICommand
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
        /// <param name="parameter">Not in use</param>
        /// <returns>Always true</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Gets the current config from Configuration.Instance property, change the config properties
        /// to the current values from the DataContext and saves the configuration including the selected lights
        /// </summary>
        /// <param name="parameter">Not in use</param>
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
