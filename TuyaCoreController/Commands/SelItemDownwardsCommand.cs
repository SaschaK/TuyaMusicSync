using System;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    /// <summary>
    /// Selected item downwards command
    /// </summary>
    public class SelItemDownwardsCommand : ICommand
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
        /// Move the currently selected item from the selected lights collection downwards (SelectedIdxSelLights + 1)
        /// </summary>
        /// <param name="parameter">Not in use</param>
        public void Execute(object parameter)
        {
            if (OwnDataContext.Instance.SelectedIdxSelLights > -1 && OwnDataContext.Instance.SelectedIdxSelLights < OwnDataContext.Instance.SelectedLights.Count - 1)
                OwnDataContext.Instance.SelectedLights.Move(OwnDataContext.Instance.SelectedIdxSelLights, OwnDataContext.Instance.SelectedIdxSelLights + 1);
        }
    }
}
