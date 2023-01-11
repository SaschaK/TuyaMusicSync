using System;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    /// <summary>
    /// Delete selected light command
    /// </summary>
    public class SelItemDeleteCommand : ICommand
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
        /// Removes the currently selected light from the selected lights collection in the DataContext
        /// </summary>
        /// <param name="parameter">Not in use</param>
        public void Execute(object parameter)
        {
            if (OwnDataContext.Instance.SelectedIdxSelLights > -1 && OwnDataContext.Instance.SelectedIdxSelLights <= OwnDataContext.Instance.SelectedLights.Count - 1)
                OwnDataContext.Instance.SelectedLights.RemoveAt(OwnDataContext.Instance.SelectedIdxSelLights);
        }
    }
}
