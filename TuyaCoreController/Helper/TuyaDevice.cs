using com.clusterrr.TuyaNet;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using TuyaCoreController.Enums;

namespace TuyaCoreController
{
    /// <summary>
    /// Device class for Tuya light devices
    /// </summary>
    public class OwnTuyaDevice : INotifyPropertyChanged
    {
        /// <summary>
        /// IP-Address as String (local value from the broadcast)
        /// </summary>
        public String IP { get; set; }
        /// <summary>
        /// Tuya Device Id (cloud value)
        /// </summary>
        public String DeviceId { get; set; }
        /// <summary>
        /// Local Key (cloud value)
        /// </summary>
        public String LocalKey { get; set; }
        /// <summary>
        /// Gw Id (cloud value)
        /// </summary>
        public String GwId { get; set; }
        /// <summary>
        /// Version (cloud value)
        /// </summary>
        public String Version { get; set; }
        /// <summary>
        /// Bulb version (detected by device DPS)
        /// </summary>
        public String BulbVersion { get; set; }
        /// <summary>
        /// Device Name field
        /// </summary>
        private String name;
        /// <summary>
        /// Online state field
        /// </summary>
        private OnlineState state;
        /// <summary>
        /// LastColor field
        /// </summary>
        private Color lastColor;

        /// <summary>
        /// Name property
        /// </summary>
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Online state property
        /// </summary>
        public OnlineState State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Last Color property
        /// </summary>
        public Color LastColor
        {
            get { return lastColor; }
            set
            {
                lastColor = value;
                if (MainWindow.Instance != null)
                    MainWindow.Instance.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        OnPropertyChanged();
                    }));
            }
        }

        /// <summary>
        /// DeviceInfo as JSON from Tuya Cloud
        /// </summary>
        public String DeviceInfoJSON { get; set; }

        /// <summary>
        /// Gateway Id
        /// </summary>
        public String GatewayId { get; set; }

        /// <summary>
        /// Device is a gateway device
        /// </summary>
        public Boolean IsGateway { get; set; }

        /// <summary>
        /// Device Category (Tuya Cloud)
        /// </summary>
        public String DeviceCategory { get; set; }

        /// <summary>
        /// Connection property to allow a permanent connection to the device
        /// </summary>
        internal TuyaDevice Connection { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ip">IP-Address</param>
        /// <param name="deviceId">Device Id</param>
        /// <param name="localKey">Local Key</param>
        /// <param name="name">Name</param>
        /// <param name="bulbVersion">Bulb version</param>
        public OwnTuyaDevice(String ip, String deviceId, String localKey, /*TuyaDeviceApiInfo apiInfo = null*/String name = null, string bulbVersion = null)
        {
            IP = ip;
            DeviceId = deviceId;
            LocalKey = localKey;
            Name = name;
            //ApiInfo = apiInfo;
            BulbVersion = bulbVersion;
            Connection = null;
            State = OnlineState.Offline;
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public OwnTuyaDevice()
        { }

        /// <summary>
        /// PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method to trigger added event handler
        /// </summary>
        /// <param name="propertyName">Property name (not needed because of [CallerMemberName] from CompilerServices</param>
        public void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// ToString method
        /// </summary>
        /// <returns>Device's name property or if empty IP and GwId</returns>
        public override String ToString()
        {
            if (/*ApiInfo != null*/Name != String.Empty)
            {
                return Name;
            }
            else
            {
                return String.Format("{0} - {1}", IP, GwId);
            }
        }
    }
}
