using System;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    /// <summary>
    /// Save selected lights command
    /// Always executeable
    /// Clears the selected lights in Settings.xml, add the currently selected lights and override the Settings.xml
    /// </summary>
    public class SaveSelectedLightsCommand : ICommand
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
        /// <returns>always true</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Execute method
        /// Gets the config from Configuration.Instance property, clear the SelectedLights collection,
        /// adds the currently selected lights and save the configuration w/o changing other properties
        /// </summary>
        /// <param name="parameter">Not in use</param>
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
