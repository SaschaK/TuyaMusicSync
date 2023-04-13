using com.clusterrr.TuyaNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController
{
    /// <summary>
    /// Helper class
    /// Provides all methods to interact with Tuya light devices
    /// </summary>
    internal class TuyaHelper
    {
        /// <summary>
        /// InitLights task
        /// Detects the bulb version by device DPS and switch on the light
        /// </summary>
        /// <returns></returns>
        public static async Task InitLights()
        {
            foreach (var light in OwnDataContext.Instance.SelectedLights)
            {
                if (light.GatewayId != String.Empty)
                {
                    var gate = OwnDataContext.Instance.CloudLights.FirstOrDefault(a => a.DeviceId == light.GatewayId);
                    if (gate != null)
                        light.IP = gate.IP;
                }

                if (light.IP.ToString() != "0.0.0.0" || light.GatewayId != String.Empty)
                {
                    var dev = new TuyaDevice(light.IP.ToString(), light.LocalKey, light.DeviceId);
                    light.Connection = dev;
                    dev.PermanentConnection = true;

                    var dps = await dev.GetDpsAsync();
                    if (dps.ContainsKey(20))
                        light.BulbVersion = "B";
                    else
                        light.BulbVersion = "A";
                    System.Threading.Thread.Sleep(50);
                    byte[] request;
                    if (light.BulbVersion == "B")
                    {
                        request = dev.EncodeRequest(TuyaCommand.CONTROL, dev.FillJson("{\"dps\":{\"20\":true}}")); //An oder aus
                    }
                    else
                    {
                        request = dev.EncodeRequest(TuyaCommand.CONTROL, dev.FillJson("{\"dps\":{\"1\":true}}")); //An oder aus
                    }
                    try
                    {
                        var encryptedResponse = await dev.SendAsync(request);
                        TuyaLocalResponse response = dev.DecodeResponse(encryptedResponse);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Unbind all selected lights
        /// No more needed
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static Task UnbindLights()
        {
            foreach (var light in OwnDataContext.Instance.SelectedLights)
            {
                if (light.Connection == null)
                {
                    var dev = new TuyaDevice(light.IP.ToString(), light.LocalKey, light.DeviceId)
                    {
                        PermanentConnection = true
                    };
                    light.Connection = dev;
                }
                light.Connection.Dispose();
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Sets the audio spectrum to the corresponding light.
        /// Uses the generated color pallet and the spectrum value to find the right color and sets the light device to it
        /// </summary>
        /// <param name="spectrum">Audio spectrum list as List of byte</param>
        public static void SetSpectrum(List<byte> spectrum)
        {
            // Add default colors if the user not generated a color pallet
            if (OwnDataContext.Instance.Colors.Count == 0)
            {
                System.Drawing.Color red = System.Drawing.Color.Red;
                System.Drawing.Color green = System.Drawing.Color.Green;
                System.Drawing.Color blue = System.Drawing.Color.Blue;
                System.Drawing.Color white = System.Drawing.Color.White;
                System.Drawing.Color off = System.Drawing.Color.FromArgb(0, 0, 0);

                OwnDataContext.Instance.Colors.Add(red);
                OwnDataContext.Instance.Colors.Add(green);
                OwnDataContext.Instance.Colors.Add(blue);
                OwnDataContext.Instance.Colors.Add(white);
                OwnDataContext.Instance.Colors.Add(off);
            }

            for (int i = 0; i < spectrum.Count - 1; i++)
            {
                int difColor = 255 / OwnDataContext.Instance.Colors.Count;
                int idxColor = Convert.ToInt32(spectrum[i].ToString()) / difColor;
                if (idxColor >= OwnDataContext.Instance.Colors.Count)
                {
                    idxColor = OwnDataContext.Instance.Colors.Count - 1;
                }
                else if (idxColor < 0)
                    idxColor = 0;
                TuyaHelper.SetDeviceToRGBColor(OwnDataContext.Instance.SelectedLights[i], OwnDataContext.Instance.Colors[idxColor]);
            }
            System.Threading.Thread.Sleep(OwnDataContext.Instance.Delay);
        }

        /// <summary>
        /// Sets the provided light to white-mode
        /// </summary>
        /// <param name="light">light device</param>
        /// <returns></returns>
        public static async Task SetDeviceToWhite(OwnTuyaDevice light)
        {
            if (light.Connection == null)
            {
                var dev = new TuyaDevice(light.IP.ToString(), light.LocalKey, light.DeviceId)
                {
                    PermanentConnection = true
                };
                light.Connection = dev;
            }
            byte[] request2;
            if (light.BulbVersion == "B")
            {
                request2 = light.Connection.EncodeRequest(TuyaCommand.CONTROL, light.Connection.FillJson("{\"dps\":{\"21\":\"white\"}}")); //"Mode "white", "colour", "scene"
            }
            else
            {
                request2 = light.Connection.EncodeRequest(TuyaCommand.CONTROL, light.Connection.FillJson("{\"dps\":{\"2\":\"white\"}}")); //"Mode "white", "colour", "scene"
            }
            var encryptedResponse2 = await light.Connection.SendAsync(request2);
            TuyaLocalResponse response2 = light.Connection.DecodeResponse(encryptedResponse2);
        }

        /// <summary>
        /// Sets all selected light devices to colour-mode
        /// </summary>
        /// <returns></returns>
        public static async Task SetAllDevicesToColor()
        {
            foreach (var lightObj in OwnDataContext.Instance.SelectedLights)
            {
                try
                {
                    OwnTuyaDevice light = (OwnTuyaDevice)lightObj;
                    if (light.Connection == null)
                    {
                        var dev = new TuyaDevice(light.IP.ToString(), light.LocalKey, light.DeviceId)
                        {
                            PermanentConnection = true
                        };
                        light.Connection = dev;
                    }

                    byte[] request2;
                    if (light.BulbVersion == "B")
                    {
                        request2 = light.Connection.EncodeRequest(TuyaCommand.CONTROL, light.Connection.FillJson("{\"dps\":{\"21\":\"colour\"}}")); //"Mode "white", "colour", "scene"
                    }
                    else
                    {
                        request2 = light.Connection.EncodeRequest(TuyaCommand.CONTROL, light.Connection.FillJson("{\"dps\":{\"2\":\"colour\"}}")); //"Mode "white", "colour", "scene"
                    }
                    var encryptedResponse2 = await light.Connection.SendAsync(request2);
                    TuyaLocalResponse response2 = light.Connection.DecodeResponse(encryptedResponse2);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Sets the provided light device to the provided color
        /// </summary>
        /// <param name="light">Light device</param>
        /// <param name="color">Color</param>
        public static void SetDeviceToRGBColor(OwnTuyaDevice light, Color color)
        {
            var res = Task.Run(() =>
            {
                var divider = OwnDataContext.Instance.Divider;
                Color usedColor = Color.FromArgb(((int)color.R / divider), ((int)color.G / divider), ((int)color.B / divider));
                if (light.Connection == null)
                {
                    var dev = new TuyaDevice(light.IP.ToString(), light.LocalKey, light.DeviceId)
                    {
                        PermanentConnection = true
                    };
                    light.Connection = dev;
                }
                if (usedColor != light.LastColor)
                {
                    var hexval = RGB_HSV_Converter.rgb_to_hexvalue(usedColor.R, usedColor.G, usedColor.B, light.BulbVersion).ToLower();
                    byte[] request3;
                    if (light.BulbVersion == "B")
                        request3 = light.Connection.EncodeRequest(TuyaCommand.CONTROL, light.Connection.FillJson("{\"dps\":{\"24\":\"" + hexval + "\"}}"));
                    else
                        request3 = light.Connection.EncodeRequest(TuyaCommand.CONTROL, light.Connection.FillJson("{\"dps\":{\"5\":\"" + hexval + "\"}}"));

                    var encryptedResponse3 = light.Connection.SendAsync(request3);
                    light.LastColor = usedColor;
                }
            });
        }
    }
}
