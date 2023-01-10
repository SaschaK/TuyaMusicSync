using AudioSpectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    public class StartLightShowCommand : ICommand
    {
        Boolean _executeable = true;
        public event EventHandler CanExecuteChanged;
        public void ChangeExecuteable()
        {
            _executeable = !_executeable;
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _executeable;
        }

        public async void Execute(object parameter)
        {
            OwnDataContext.Instance.AppState = "Starting the show...";
            ChangeExecuteable();
            await TuyaHelper.InitLights();
            await TuyaHelper.SetAllDevicesToColor();
            System.Threading.Thread.Sleep(2000);
            MainWindow.Instance.analyzer = new AudioSpectrum.Analyzer(OwnDataContext.Instance.SelectedLights.Count);
            MainWindow.Instance.analyzer.Enable = true;
            OwnDataContext.Instance.StopLightShowCmd.ChangeExecuteable();
            OwnDataContext.Instance.AppState = "Running...";
        }
    }
}
