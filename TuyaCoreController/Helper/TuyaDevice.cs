using com.clusterrr.TuyaNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using TuyaCoreController.Enums;

namespace TuyaCoreController
{
    // Tuya-Device class
    public class OwnTuyaDevice : INotifyPropertyChanged
    {
        public String IP { get; set; }
        public String DeviceId { get; set; }
        public String LocalKey { get; set; }
        public String GwId { get; set; }
        public String Version { get; set; }
        public String BulbVersion { get; set; }
        private String name;
        private OnlineState state;
        private Color lastColor;

        public String Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        public OnlineState State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged();
            }
        }

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

        internal TuyaDevice Connection { get; set; }

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

        public OwnTuyaDevice()
        { }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] String propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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
