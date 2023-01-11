using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.Threading;
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
    }
}
