using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;
using TuyaCoreController.Commands;
using Un4seen.BassWasapi;

namespace TuyaCoreController.ViewModel
{
    public class OwnDataContext : INotifyPropertyChanged
    {
        #region Static fields
        public static OwnDataContext Instance { get; private set; }
        #endregion

        #region Private fields
        private bool useTuyaCloud = true;
        private string tuyaAccessId = "";
        private string tuyaSecret = "";
        private int divider = 1;
        private int countOfColors = 0;
        private int countOfSelLights = 0;
        private ObservableCollection<byte> spectrum = new();
        private ObservableCollection<OwnTuyaDevice> selectedLights = new();
        private ObservableCollection<OwnTuyaDevice> cloudLights = new();
        private ObservableCollection<KeyValuePair<int, Un4seen.BassWasapi.BASS_WASAPI_DEVICEINFO>> outputDevices = new();
        private KeyValuePair<int, Un4seen.BassWasapi.BASS_WASAPI_DEVICEINFO> selectedDevice;
        public List<Color> Colors { get; set; } = new();
        private int tuyaRegion = 0;
        private String tuyaAnyDevice = "";
        private int delay = 300;
        private String appState = "";
        #endregion

        #region Event handling
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion

        #region Constructor
        public OwnDataContext()
        {
            Instance = this;
            Divider = 1;
            CountOfColors = 5;
            SelectedLights = new();
            SelectedLights.CollectionChanged += SelectedLights_CollectionChanged;
            CloudLights = new();
            CloudLights.CollectionChanged += CloudLights_CollectionChanged;
            OutputDevices = new();
            OutputDevices.CollectionChanged += OutputDevices_CollectionChanged;
            Spectrum = new();
            Spectrum.CollectionChanged += Spectrum_CollectionChanged;
            RetrieveLightsCmd = new();
            SelectLightsCmd = new();
            GenColorPalCmd = new();
            SaveSetCmd = new();
            SaveSelectedLightsCmd = new();
            StartLightShowCmd = new();
            StopLightShowCmd = new();
            SelItemUpwardsCmd = new();
            SelItemDownwardsCmd = new();
            SelItemDeleteCmd = new();
            Colors = new();
            AppState = "Idle";

            var config = Configuration.Instance;
            TuyaRegion = config.TuyaRegion;
            TuyaAccessId = config.TuyaAccessId;
            TuyaSecret = config.TuyaAccessSecret;
            Colors = config.Colors;
            CountOfColors = config.Colors.Count;
            UseTuyaCloud = config.UseTuyaCloud;
            TuyaAnyDevice = config.TuyaAnyDevice;
            Delay = config.Delay;

            //Sound output devices
            var tGetDevices = Task.Run(() => GetSoundDevices());
        }
        #endregion

        #region Async Tasks
        internal Task GetSoundDevices()
        {
            for (int i = 0; i < BassWasapi.BASS_WASAPI_GetDeviceCount(); i++)
            {
                var device = BassWasapi.BASS_WASAPI_GetDeviceInfo(i);
                if (device.IsEnabled && device.IsLoopback)
                {
                    var a = i;
                    MainWindow.Instance.Dispatcher.Invoke(() =>
                    {
                        OutputDevices.Add(new KeyValuePair<int, BASS_WASAPI_DEVICEINFO>(a, device));
                    });
                }
            }
            MainWindow.Instance.Dispatcher.BeginInvoke(new Action(() =>
            {
                var sd = OutputDevices.FirstOrDefault(a => a.Key == Configuration.Instance.SoundDevice);
                if (sd.Equals(default(KeyValuePair<int, Un4seen.BassWasapi.BASS_WASAPI_DEVICEINFO>)) == false)
                {
                    OwnDataContext.Instance.SelectedDevice = sd;
                }
            }));
            return Task.CompletedTask;
        }
        #endregion

        #region Properties
        public bool UseTuyaCloud
        {
            get { return useTuyaCloud; }
            set
            {
                useTuyaCloud = value;
                OnPropertyChanged();
            }
        }

        public string TuyaAccessId
        {
            get { return tuyaAccessId; }
            set
            {
                tuyaAccessId = value;
                OnPropertyChanged();
            }
        }

        public string TuyaSecret
        {
            get { return tuyaSecret; }
            set { tuyaSecret = value; }
        }

        public int Divider
        {
            get { return divider; }
            set
            {
                divider = value;
                OnPropertyChanged();
            }
        }

        public int CountOfColors
        {
            get { return countOfColors; }
            set
            {
                countOfColors = value;
                OnPropertyChanged();
            }
        }

        public int CountOfSelLights
        {
            get { return countOfSelLights; ; }
            set
            {
                countOfSelLights = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<byte> Spectrum
        {
            get { return spectrum; }
            set
            {
                spectrum = value;
                OnPropertyChanged();
            }
        }

        public int TuyaRegion
        {
            get { return tuyaRegion; }
            set
            {
                tuyaRegion = value;
                OnPropertyChanged();
            }
        }

        public String TuyaAnyDevice
        {
            get { return tuyaAnyDevice; }
            set
            {
                tuyaAnyDevice = value;
                OnPropertyChanged();
            }
        }

        public int Delay
        {
            get { return delay; }
            set
            {
                delay = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OwnTuyaDevice> SelectedLights
        {
            get { return selectedLights; }
            set
            {
                selectedLights = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OwnTuyaDevice> CloudLights
        {
            get { return cloudLights; }
            set { cloudLights = value; }
        }

        public ObservableCollection<KeyValuePair<int, Un4seen.BassWasapi.BASS_WASAPI_DEVICEINFO>> OutputDevices
        {
            get { return outputDevices; }
            set
            {
                outputDevices = value;
                OnPropertyChanged();
            }
        }

        public String AppState
        {
            get { return appState; }
            set
            {
                appState = value;
                OnPropertyChanged();
            }
        }

        public KeyValuePair<int, Un4seen.BassWasapi.BASS_WASAPI_DEVICEINFO> SelectedDevice
        {
            get { return selectedDevice; }
            set
            {
                selectedDevice = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIdxSelLights { get; set; }
        #endregion

        #region Command Properties
        public RetrieveLightsCommand RetrieveLightsCmd { get; set; }
        public SelectLightCommand SelectLightsCmd { get; set; }
        public GenerateColorPalletCommand GenColorPalCmd { get; set; }
        public SaveSettingsCommand SaveSetCmd { get; set; }
        public SaveSelectedLightsCommand SaveSelectedLightsCmd { get; set; }
        public StartLightShowCommand StartLightShowCmd { get; set; }
        public StopLightShowCommand StopLightShowCmd { get; set; }
        public SelItemUpwardsCommand SelItemUpwardsCmd { get; set; }
        public SelItemDownwardsCommand SelItemDownwardsCmd { get; set; }
        public SelItemDeleteCommand SelItemDeleteCmd { get; set; }
        #endregion

        #region ObservableCollection Changed handler
        private void SelectedLights_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("SelectedLights");
            CountOfSelLights = SelectedLights.Count;
        }

        private void CloudLights_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("CloudLights");
        }

        private void OutputDevices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("OutputDevices");
        }

        private void Spectrum_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Spectrum");
        }
        #endregion

    }
}
