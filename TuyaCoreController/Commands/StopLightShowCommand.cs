using System;
using System.Windows.Input;
using System.Windows.Threading;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    /// <summary>
    /// Stop the light show command
    /// </summary>
    public class StopLightShowCommand : ICommand
    {
        Boolean _executeable = false;

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
        /// Sets the AppState property in the DataContext. Disabling the Audio spectrum analyzer and
        /// sets all selected lights to white-mode
        /// </summary>
        /// <param name="parameter">Not in use</param>
        public async void Execute(object parameter)
        {
            await Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                OwnDataContext.Instance.AppState = "Stopping the show...";
            }));
            ChangeExecuteable();
            var analyzer = MainWindow.Instance.analyzer;
            if (analyzer != null)
            {
                analyzer.Enable = false;
                analyzer.Free();
            }
            foreach (var item in OwnDataContext.Instance.SelectedLights)
            {
                var light = item;
                var result = TuyaHelper.SetDeviceToWhite(light);
                System.Threading.Thread.Sleep(200);
                var result2 = TuyaHelper.SetDeviceToWhite(light);
            }
            OwnDataContext.Instance.StartLightShowCmd.ChangeExecuteable();
            await Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
            {
                OwnDataContext.Instance.AppState = "Idle";
            }));
        }
    }
}
