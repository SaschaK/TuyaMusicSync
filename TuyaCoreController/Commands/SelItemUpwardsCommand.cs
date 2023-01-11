using System;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    /// <summary>
    /// Selected item upwards command
    /// </summary>
    public class SelItemUpwardsCommand : ICommand
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
        /// Move the currently selected item from the selected lights collection upwards (SelectedIdxSelLights - 1)
        /// </summary>
        /// <param name="parameter">Not in use</param>
        public void Execute(object parameter)
        {
            if (OwnDataContext.Instance.SelectedIdxSelLights > -1 && OwnDataContext.Instance.SelectedIdxSelLights > 0)
                OwnDataContext.Instance.SelectedLights.Move(OwnDataContext.Instance.SelectedIdxSelLights, OwnDataContext.Instance.SelectedIdxSelLights - 1);
        }
    }
}
