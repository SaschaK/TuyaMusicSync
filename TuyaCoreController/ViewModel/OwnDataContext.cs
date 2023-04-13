using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using TuyaCoreController.Commands;
using Un4seen.BassWasapi;

namespace TuyaCoreController.ViewModel
{
    /// <summary>
    /// DataContext class
    /// </summary>
    public class OwnDataContext : INotifyPropertyChanged
    {
        #region Static fields
        /// <summary>
        /// Instance property
        /// </summary>
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
        /// <summary>
        /// PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Method to trigger all attached event handler
        /// </summary>
        /// <param name="property">Not needed because of [CallerMemberName] from CompilerServices</param>
        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor - Initialize almost all properties and loads the config
        /// </summary>
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
        /// <summary>
        /// Task to detection as enabled Loopback devices
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Use Tuya cloud property
        /// </summary>
        public bool UseTuyaCloud
        {
            get { return useTuyaCloud; }
            set
            {
                useTuyaCloud = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Tuya access id property
        /// </summary>
        public string TuyaAccessId
        {
            get { return tuyaAccessId; }
            set
            {
                tuyaAccessId = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Tuya access secret property
        /// </summary>
        public string TuyaSecret
        {
            get { return tuyaSecret; }
            set { tuyaSecret = value; }
        }

        /// <summary>
        /// Integer property to reduce the light's brightness by division
        /// </summary>
        public int Divider
        {
            get { return divider; }
            set
            {
                divider = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Count of colors property
        /// </summary>
        public int CountOfColors
        {
            get { return countOfColors; }
            set
            {
                countOfColors = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Count of selected lights property
        /// </summary>
        public int CountOfSelLights
        {
            get { return countOfSelLights; ; }
            set
            {
                countOfSelLights = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// ObservableCollection property for the audio spectrum bytes 
        /// </summary>
        public ObservableCollection<byte> Spectrum
        {
            get { return spectrum; }
            set
            {
                spectrum = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Tuya region property
        /// </summary>
        public int TuyaRegion
        {
            get { return tuyaRegion; }
            set
            {
                tuyaRegion = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property for one of your device ids.
        /// </summary>
        public String TuyaAnyDevice
        {
            get { return tuyaAnyDevice; }
            set
            {
                tuyaAnyDevice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Delay property between the color changes
        /// </summary>
        public int Delay
        {
            get { return delay; }
            set
            {
                delay = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// ObservableCollection property for the selected Lights
        /// </summary>
        public ObservableCollection<OwnTuyaDevice> SelectedLights
        {
            get { return selectedLights; }
            set
            {
                selectedLights = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// ObservableCollection property for the cloud Lights
        /// </summary>
        public ObservableCollection<OwnTuyaDevice> CloudLights
        {
            get { return cloudLights; }
            set { cloudLights = value; }
        }

        /// <summary>
        /// ObservableCollection property for the sound devices (key is used in the analyzer to activate the right audio output device)
        /// </summary>
        public ObservableCollection<KeyValuePair<int, Un4seen.BassWasapi.BASS_WASAPI_DEVICEINFO>> OutputDevices
        {
            get { return outputDevices; }
            set
            {
                outputDevices = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// AppState property to set the AppState visible to the user
        /// </summary>
        public String AppState
        {
            get { return appState; }
            set
            {
                appState = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property for the selected audio device
        /// </summary>
        public KeyValuePair<int, Un4seen.BassWasapi.BASS_WASAPI_DEVICEINFO> SelectedDevice
        {
            get { return selectedDevice; }
            set
            {
                selectedDevice = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property for the index of the currently selected "SelectedLights" item
        /// </summary>
        public int SelectedIdxSelLights { get; set; }

        public Boolean Debug { get; set; } = false;
        #endregion

        #region Command Properties
        /// <summary>
        /// SelectedLights Command property
        /// </summary>
        public SelectLightCommand SelectLightsCmd { get; set; }
        /// <summary>
        /// Generate color pallet command property
        /// </summary>
        public GenerateColorPalletCommand GenColorPalCmd { get; set; }
        /// <summary>
        /// Save Settings command property
        /// </summary>
        public SaveSettingsCommand SaveSetCmd { get; set; }
        /// <summary>
        /// Save selected light command property
        /// </summary>
        public SaveSelectedLightsCommand SaveSelectedLightsCmd { get; set; }
        /// <summary>
        /// Start the light show command property
        /// </summary>
        public StartLightShowCommand StartLightShowCmd { get; set; }
        /// <summary>
        /// Stop the light show command property
        /// </summary>
        public StopLightShowCommand StopLightShowCmd { get; set; }
        /// <summary>
        /// Selected light upwards command property
        /// </summary>
        public SelItemUpwardsCommand SelItemUpwardsCmd { get; set; }
        /// <summary>
        /// Selected light downwardds command
        /// </summary>
        public SelItemDownwardsCommand SelItemDownwardsCmd { get; set; }
        /// <summary>
        /// Delete selected items collection property
        /// </summary>
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
