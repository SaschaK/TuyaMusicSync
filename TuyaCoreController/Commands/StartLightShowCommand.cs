using System;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    /// <summary>
    /// Start light show command
    /// </summary>
    public class StartLightShowCommand : ICommand
    {
        Boolean _executeable = true;
        /// <summary>
        /// CanExecuteChanged event
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Reverse the executeability of the command and triggers the event handler
        /// </summary>
        public void ChangeExecuteable()
        {
            _executeable = !_executeable;
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Returns the executeability
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>if the command is executeable</returns>
        public bool CanExecute(object parameter)
        {
            return _executeable;
        }

        /// <summary>
        /// Execute method
        /// Sets the AppState property in the DataContext, initialize all selected lights (turn them on)
        /// and sets them to colour-mode. Audio spectrum analyzer will be initialized and enabled
        /// </summary>
        /// <param name="parameter">Not in use</param>
        public async void Execute(object parameter)
        {
            OwnDataContext.Instance.AppState = "Starting the show...";
            ChangeExecuteable();
            await TuyaHelper.InitLights();
            await TuyaHelper.SetAllDevicesToColor();
            System.Threading.Thread.Sleep(2000);
            MainWindow.Instance.analyzer = new AudioSpectrum.Analyzer(OwnDataContext.Instance.SelectedLights.Count + 1);
            MainWindow.Instance.analyzer.Enable = true;
            OwnDataContext.Instance.StopLightShowCmd.ChangeExecuteable();
            OwnDataContext.Instance.AppState = "Running...";
        }
    }
}
