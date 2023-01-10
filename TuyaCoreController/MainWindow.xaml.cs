using com.clusterrr.TuyaNet;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static MainWindow Instance { get; set; }
        internal AudioSpectrum.Analyzer analyzer;

        public MainWindow()
        {
            Closing += MainWindow_Closing;
            Instance = this;
            InitializeComponent();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Process.GetCurrentProcess().Kill();
        }
    }
}
