using com.clusterrr.TuyaNet;
using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Media.Media3D;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        /// <summary>
        /// Instance property
        /// </summary>
        public static MainWindow Instance { get; set; }
        internal AudioSpectrum.Analyzer analyzer;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            Closing += MainWindow_Closing;
            Instance = this;
            InitializeComponent();
        }

        /// <summary>
        /// Event handler to react on the MainWindow Closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // Disable and dispose the analyzer instance
                if (analyzer != null)
                {
                    analyzer.Enable = false;
                    analyzer.Free();
                }

                // Set all selected lights to white-Mode 2 times (somtimes the white-mode command is not working)
                foreach (var item in OwnDataContext.Instance.SelectedLights)
                {
                    var light = item;
                    var result = TuyaHelper.SetDeviceToWhite(light);
                    System.Threading.Thread.Sleep(200);
                    var result2 = TuyaHelper.SetDeviceToWhite(light);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            App.Instance.scanner.Stop();
        }

        /*private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            foreach (var light in OwnDataContext.Instance.CloudLights)
            {

                if (light.GatewayId != String.Empty)
                {
                    var gate = OwnDataContext.Instance.CloudLights.FirstOrDefault(a => a.DeviceId == light.GatewayId);
                    if (gate != null)
                    {
                        var gateRes = await App.Instance.Api.RequestAsync(TuyaApi.Method.GET, $"/v1.0/devices/{gate.DeviceId}/functions");
                        var gateRes2 = await App.Instance.Api.RequestAsync(TuyaApi.Method.GET, $"/v1.0/functions/{gate.DeviceCategory}");
                        //light.IP = gate.IP;
                        //light.LocalKey= gate.LocalKey;
                    }

                    var dev = new TuyaDevice(light.IP, light.LocalKey, light.DeviceId);
                    light.Connection = dev;
                    dev.PermanentConnection = true;

                    var res = await App.Instance.Api.RequestAsync(TuyaApi.Method.GET, $"/v1.0/iot-03/devices/{light.DeviceId}/status");
                    //var dps = await dev.GetDpsAsync();
                }
            }
        }*/
    }
}
