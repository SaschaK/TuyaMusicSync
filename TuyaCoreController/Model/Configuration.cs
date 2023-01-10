using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace TuyaCoreController
{
    public class Configuration
    {
        internal static Configuration Instance { get; set; }
        public List<OwnTuyaDevice> Devices { get; set; }
        public Boolean UseTuyaCloud { get; set; }
        public int TuyaRegion { get; set; }
        public String TuyaAccessId { get; set; }
        public String TuyaAccessSecret { get; set; }
        public String TuyaAnyDevice { get; set; }
        public int CountOfColors { get; set; }
        public List<String> SelectedLightIds { get; set; }
        public List<String> ColorStrings { get; set; }
        internal List<Color> Colors { get; set; }
        public int Delay { get; set; }
        public int SoundDevice { get; set; }

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
