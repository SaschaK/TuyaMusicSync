using com.clusterrr.TuyaNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    /// <summary>
    /// Used in V0.1 - No more needed
    /// Replaced by PropertyChanged event and OnlineStatusEnum in class OwnTuyaDevice
    /// Retrieve online lights and refresh CloudLights Collection in the DataContext
    /// </summary>
    [Obsolete("Don't use this command anymore")]
    public class RetrieveLightsCommand : ICommand
    {
        /// <summary>
        /// CanExecuteChanged event
        /// </summary>
        public event EventHandler CanExecuteChanged;
        private Boolean _executeable = true;
        /// <summary>
        /// Default ICommand method
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>if the command is executeable or not</returns>
        public bool CanExecute(object parameter)
        {
            return _executeable;
        }

        /// <summary>
        /// Can not be executed when the online lights detection in already running
        /// </summary>
        /// <param name="executeable">Can the command be executed?</param>
        public void ChangeExecuteability(Boolean executeable)
        {
            _executeable = executeable;
            CanExecuteChanged(this, new EventArgs());
        }

        /// <summary>
        /// Execute method
        /// Change the command executability and detect online lights and the bulb version
        /// </summary>
        /// <param name="parameter">Not in use</param>
        public async void Execute(object parameter)
        {
            ChangeExecuteability(false);
            List<OwnTuyaDevice> devicesToCheck = new List<OwnTuyaDevice>(OwnDataContext.Instance.CloudLights);
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

