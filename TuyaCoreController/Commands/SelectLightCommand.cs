using System;
using System.Linq;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    /// <summary>
    /// Select light command
    /// </summary>
    public class SelectLightCommand : ICommand
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
        /// Adds the selected lights from the "cloud / local lights" list to the selected lights
        /// </summary>
        /// <param name="parameter">Selected light</param>
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
