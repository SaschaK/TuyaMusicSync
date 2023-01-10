using AudioSpectrum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    public class StopLightShowCommand : ICommand
    {
        Boolean _executeable = false;
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
