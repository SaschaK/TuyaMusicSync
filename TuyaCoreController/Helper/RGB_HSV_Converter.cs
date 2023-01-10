using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;

namespace TuyaCoreController
{
    // manually translated python lib to convert the colors correctly
    public static class RGB_HSV_Converter
    {
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public static void hexvalue_to_hsv(String hexvalue, out double h, out double s, out double v, string bulb = "A")
        {
            h = 0;
            s = 0;
            v = 0;
            if (bulb == "A")
            {
                h = Convert.ToInt32(hexvalue.Substring(7, 3), 16) / 360.0;
                s = Convert.ToInt32(hexvalue.Substring(10, 2), 16) / 255.0;
                v = Convert.ToInt32(hexvalue.Substring(12, 2), 16) / 255.0;
            }

            if (bulb == "B")
            {
                h = Convert.ToInt32(hexvalue.Substring(0, 4), 16) / 360.0;
                s = Convert.ToInt32(hexvalue.Substring(4, 4), 16) / 1000.0;
                v = Convert.ToInt32(hexvalue.Substring(8, 4), 16) / 1000.0;
            }
        }

        public static void hexvalue_to_rgb(string hexvalue, string bulb = "A")
        {

            int r, g, b;
            double h, s, v;
            if (bulb == "A")
            {
                r = Convert.ToInt32(hexvalue.Substring(0, 2), 16);
                g = Convert.ToInt32(hexvalue.Substring(2, 2), 16);
                b = Convert.ToInt32(hexvalue.Substring(4, 2), 16);
            }
            if (bulb == "B")
            {
                h = float.Parse(Convert.ToInt32(hexvalue.Substring(0, 4), 16).ToString()) / 360.0;
                s = float.Parse(Convert.ToInt32(hexvalue.Substring(4, 4), 16).ToString()) / 1000.0;
                v = float.Parse(Convert.ToInt32(hexvalue.Substring(8, 4), 16).ToString()) / 1000.0;

                var rgb = hsv_to_rgb(h, s, v);
                r = Convert.ToInt32(rgb[0] * 255);
                g = Convert.ToInt32(rgb[1] * 255);
                b = Convert.ToInt32(rgb[2] * 255);

            }
        }

        public static double[] hsv_to_rgb(double h, double s, double v)
        {
            if (s == 0.0)
                return new double[3] { v, v, v };

            var i = Convert.ToInt32((h * 6.0));
            var f = (h * 6.0) - i;
            var p = v * (1.0 - s);
            var q = v * (1.0 - s * f);
            var t = v * (1.0 - s * (1.0 - f));
            i = i % 6;
            if (i == 0)
                return new double[3] { v, t, p };
            if (i == 1)
                return new double[3] { q, v, p };
            if (i == 2)
                return new double[3] { p, v, t };
            if (i == 3)
                return new double[3] { p, q, v };
            if (i == 4)
                return new double[3] { t, p, v };
            if (i == 5)
                return new double[3] { v, p, q };

            return new double[3];
        }

        public static double[] rgb_to_hsv(double r, double g, double b)
        {
            double h, s, v;
            var maxc = Math.Max(r, Math.Max(g, b));
            var minc = Math.Min(r, Math.Min(g, b));
            var rangec = (maxc - minc);
            v = maxc;
            if (minc == maxc)
                return new double[3] { 0.0, 0.0, v };

            s = rangec / maxc;
            var rc = (maxc - r) / rangec;
            var gc = (maxc - g) / rangec;
            var bc = (maxc - b) / rangec;
            if (r == maxc)
                h = bc - gc;
            else if (g == maxc)
                h = 2.0 + rc - bc;
            else
            {
                h = 4.0 + gc - rc;
                h = (h / 6.0) % 1.0;
            }
            return new double[3] { h, s, v };
        }


        public static String rgb_to_hexvalue(int r, int g, int b, String bulb = "A")
        {
            var rgb = new int[3] { r, g, b };
            String hexvalue = "";
            var hsv = rgb_to_hsv(rgb[0] / 255.0, rgb[1] / 255.0, rgb[2] / 255.0);

            // Bulb Type A
            var hsvarray = new int[3] { Convert.ToInt32(hsv[0] * 360), Convert.ToInt32(hsv[1] * 255), Convert.ToInt32(hsv[2] * 255) };
            if (bulb == "A")
            {
                foreach (var value in rgb)
                {
                    var val = value;
                    if (val < 0)
                        val = val * -1;
                    var temp = val.ToString("X").Replace("0x", "");
                    if (temp.Length == 1)
                    {
                        temp = "0" + temp;
                        hexvalue = hexvalue + temp;
                    }
                }

                var hexvalue_hsv = "";
                foreach (var value in hsvarray)
                {
                    var val = value;
                    if (val < 0)
                        val = val * -1;
                    var temp = val.ToString("X").Replace("0x", "");
                    if (temp.Length == 1)
                    {
                        temp = "0" + temp;
                        hexvalue_hsv = hexvalue_hsv + temp;
                    }
                }

                if (hexvalue_hsv.Length == 7)
                    hexvalue = hexvalue + "0" + hexvalue_hsv;
                else
                    hexvalue = hexvalue + "00" + hexvalue_hsv;
            }

            if (bulb == "B")
            {
                hsvarray = new int[3] { Convert.ToInt32(hsv[0] * 360), Convert.ToInt32(hsv[1] * 1000), Convert.ToInt32(hsv[2] * 1000) };
                String temp = "";
                foreach (var value in hsvarray)
                {
                    var val = value;
                    if (val < 0)
                        val = val * -1;
                    temp = val.ToString("X").Replace("0x", "");
                    while (temp.Length < 4)
                        temp = "0" + temp;
                    hexvalue = hexvalue + temp;
                }

            }
            return hexvalue;
    }
    }
}
