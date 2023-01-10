using com.clusterrr.TuyaNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController
{
    internal class TuyaHelper
    {
        // Init lights
        // Create the permanent connection to avoid not needed connection establishment and turn on the lights
        public static async Task InitLights()
        {
            foreach (var light in OwnDataContext.Instance.SelectedLights)
            {
                if (light.IP.ToString() != "0.0.0.0")
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

        // Unbinding the lights
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

        // Set every light to the provided music spectrum
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

        // Set provided light to White-Mode
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

        // Set all selected lights to Colour-Mode
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

        // Set provided light to the provided color
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
