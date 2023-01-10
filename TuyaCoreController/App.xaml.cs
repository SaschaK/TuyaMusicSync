using com.clusterrr.TuyaNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using TuyaCoreController.Enums;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static App Instance;
        public Configuration Config { get; set; }
        internal System.Threading.Thread tScanner;
        public App()
        {
            Instance = this;
            Config = Configuration.Load();

            Task.Run(() => GetDevices());
        }

        Task GetDevices()
        {
            if (Config.UseTuyaCloud && Config.TuyaAccessId != String.Empty && Config.TuyaAccessSecret != String.Empty)
            {
                var api = new TuyaApi(((TuyaApi.Region)Config.TuyaRegion), Config.TuyaAccessId, Config.TuyaAccessSecret);
                var devices = api.GetAllDevicesInfoAsync(anyDeviceId: Config.TuyaAnyDevice);
                var lstDevices = new List<TuyaDeviceApiInfo>(devices.Result);
                foreach (var device in lstDevices.FindAll(a => a.Category == "dj"))
                {
                    TuyaCoreController.MainWindow.Instance.Dispatcher.BeginInvoke(delegate ()
                    {
                        String ip = "0.0.0.0";
                        var knownDev = Config.Devices.FirstOrDefault(a => a.DeviceId == device.Id);
                        if (knownDev != null)
                        {
                            ip = knownDev.IP;
                            Ping ping = new Ping();
                            if (ip != "0.0.0.0")
                            {
                                var res = ping.Send(ip, 1000);
                                if (res.Status == IPStatus.Success)
                                    knownDev.State = OnlineState.Online;
                                else
                                    knownDev.State = OnlineState.Offline;
                            }
                            else
                            {
                                knownDev.State = OnlineState.Offline;
                            }
                        }
                        OwnDataContext.Instance.CloudLights.Add(new OwnTuyaDevice("0.0.0.0", device.Id, device.LocalKey, device.Name));
                    });
                }
                TuyaCoreController.MainWindow.Instance.Dispatcher.BeginInvoke(delegate ()
                {
                    foreach (var selLight in Config.SelectedLightIds)
                    {
                        var light = OwnDataContext.Instance.CloudLights.FirstOrDefault(a => a.DeviceId == selLight);
                        if (light != null)
                            OwnDataContext.Instance.SelectedLights.Add(light);
                    }
                });
            }

            tScanner = new System.Threading.Thread(new System.Threading.ThreadStart(delegate ()
            {
                var scanner = new TuyaScanner();
                scanner.OnNewDeviceInfoReceived += Scanner_OnNewDeviceInfoReceived;
                scanner.Start();
            }));
            tScanner.Start();
            return Task.CompletedTask;
        }

        private void Scanner_OnNewDeviceInfoReceived(object sender, TuyaDeviceScanInfo e)
        {
            if (Config.UseTuyaCloud)
            {
                var ownDev = OwnDataContext.Instance.CloudLights.FirstOrDefault(a => a.DeviceId == e.GwId);
                if (ownDev != null)
                {
                    ownDev.IP = IPAddress.Parse(e.IP).ToString();
                    ownDev.State = OnlineState.Online;
                }
            }
            else
            {
                Dispatcher.BeginInvoke(delegate ()
                {
                    OwnDataContext.Instance.CloudLights.Add(new OwnTuyaDevice(e.IP, e.GwId, ""));
                });
            }
        }
    }
}
