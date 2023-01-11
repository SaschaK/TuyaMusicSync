using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace TuyaCoreController
{
    /// <summary>
    /// Configuration class to save the config as Settings.xml in the application folder
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Instance property - Set in constructor
        /// </summary>
        internal static Configuration Instance { get; set; }
        /// <summary>
        /// List of devices
        /// </summary>
        public List<OwnTuyaDevice> Devices { get; set; }
        /// <summary>
        /// Use Tuya cloud boolean
        /// </summary>
        public Boolean UseTuyaCloud { get; set; }
        /// <summary>
        /// Tuya Region integer
        /// </summary>
        public int TuyaRegion { get; set; }
        /// <summary>
        /// Tuya Access Id
        /// </summary>
        public String TuyaAccessId { get; set; }
        /// <summary>
        /// Tuya Access Secret
        /// </summary>
        public String TuyaAccessSecret { get; set; }
        /// <summary>
        /// Device Id of one of your devices
        /// </summary>
        public String TuyaAnyDevice { get; set; }
        /// <summary>
        /// Count of colors
        /// </summary>
        public int CountOfColors { get; set; }
        /// <summary>
        /// Ids of the selected lights - Easier to save than the entire OwnTuyaDevice object
        /// </summary>
        public List<String> SelectedLightIds { get; set; }
        /// <summary>
        /// Colors as String - Easier to save than the entire color object
        /// </summary>
        public List<String> ColorStrings { get; set; }
        /// <summary>
        /// List of colors - only for internal useage
        /// </summary>
        internal List<Color> Colors { get; set; }
        /// <summary>
        /// Delay
        /// </summary>
        public int Delay { get; set; }
        /// <summary>
        /// Sound Device Index
        /// </summary>
        public int SoundDevice { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public Configuration()
        {
            Instance = this;
            Devices = new List<OwnTuyaDevice>();
            UseTuyaCloud = true;
            TuyaAccessId = null;
            TuyaAccessSecret = null;
            CountOfColors = 0;
            SelectedLightIds = new List<String>();
            Colors = new List<Color>();
            ColorStrings = new List<String>();
            Delay = 300;
            SoundDevice = 0;
        }

        /// <summary>
        /// Static Load method
        /// </summary>
        /// <returns>Configuration object</returns>
        public static Configuration Load()
        {
            Configuration res = new Configuration();
            try
            {
                if (File.Exists("Settings.xml") == false)
                {
                    Save();
                }
                using (StreamReader sr = new StreamReader("Settings.xml"))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Configuration));
                    res = ((Configuration)ser.Deserialize(sr));
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                var title = Properties.Settings.Default.Title;
                MessageBox.Show(ex.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                Save();
            }
            Instance = res;
            res.Colors.Clear();

            foreach (var item in res.ColorStrings)
            {
                string[] itms = item.Split(',');
                res.Colors.Add(Color.FromArgb(Convert.ToInt32(itms[0]), Convert.ToInt32(itms[1]), Convert.ToInt32(itms[2])));
            }
            return res;
        }

        /// <summary>
        /// Static Save method - Saves the Configuration from the Instance property
        /// </summary>
        public static void Save()
        {
            try
            {
                Instance.ColorStrings.Clear();
                foreach (var item in Instance.Colors)
                {
                    Instance.ColorStrings.Add(String.Format("{0},{1},{2}", item.R, item.G, item.B));
                }
                using (StreamWriter sw = new StreamWriter("Settings.xml", false))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Configuration));
                    ser.Serialize(sw, Instance);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                var title = Properties.Settings.Default.Title;
                MessageBox.Show(ex.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
