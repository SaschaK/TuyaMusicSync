using com.clusterrr.TuyaNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    public class RetrieveLightsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Boolean _executeable = true;
        public bool CanExecute(object parameter)
        {
            return _executeable;
        }

        public void ChangeExecuteability(Boolean executeable)
        {
            _executeable = executeable;
            CanExecuteChanged(this, new EventArgs());
        }

        public async void Execute(object parameter)
        {
            ChangeExecuteability(false);
            List<OwnTuyaDevice> devicesToCheck = new List<OwnTuyaDevice>(OwnDataContext.Instance.CloudLights);
            //var devices = App.Instance.ownDevices;
            OwnDataContext.Instance.CloudLights.Clear();
            foreach (var ownDev in devicesToCheck)
            {
                if (ownDev.IP != "0.0.0.0")
                    try
                    {
                        OwnDataContext.Instance.CloudLights.Add(ownDev);
                        using (TuyaDevice dev = new TuyaDevice(ownDev.IP.ToString(), ownDev.LocalKey, ownDev.DeviceId))
                        {
                            var dps = await dev.GetDpsAsync();
                            if (dps.ContainsKey(20))
                                ownDev.BulbVersion = "B";
                            else
                                ownDev.BulbVersion = "A";
                        }
                    }
                    catch (Exception ex) {
                        Console.WriteLine(ex.Message);
                    }
            }
            List<OwnTuyaDevice> sort = new List<OwnTuyaDevice>(OwnDataContext.Instance.CloudLights);
            OwnDataContext.Instance.CloudLights.Clear();
            foreach (var item in sort.OrderBy(a => a.Name))
            {
                OwnDataContext.Instance.CloudLights.Add(item);
            }
            foreach (var item in devicesToCheck)
            {
                if (OwnDataContext.Instance.CloudLights.FirstOrDefault(a => a.DeviceId == item.DeviceId) == null)
                {
                    OwnDataContext.Instance.CloudLights.Add(item);
                }
                OwnDataContext.Instance.OnPropertyChanged("CloudLights");
            }
            ChangeExecuteability(true);
        }
    }
}

