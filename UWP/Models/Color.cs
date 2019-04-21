using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.Models
{
    class Color
    {
        private int R { get; set; }
        private int G { get; set; }
        private int B { get; set; }
        private string hex;
        public string RGB {
            get => hex;
            set {
                hex = value;
                R = Convert.ToInt32(hex.Substring(1, 2),16);
                G = Convert.ToInt32(hex.Substring(3, 2),16); 
                B = Convert.ToInt32(hex.Substring(5, 2),16);
            }
        }
        public string getTextRGB()
        {
            return "R:" + R.ToString().PadRight(6,' ') + "G:" + G.ToString().PadRight(6, ' ') + "B:" + B.ToString().PadRight(6, ' ');
        }
        public string getTextHSV()
        {
            System.Drawing.Color color = System.Drawing.Color.FromArgb(R, G, B);
            int hue = (int)color.GetHue();
            int saturation = (int)(color.GetSaturation()*100);
            int brightness = (int)(color.GetBrightness()*100);
            return "H:" + hue.ToString().PadRight(6, ' ') + "S:" + (saturation.ToString()+"%").PadRight(6, ' ') + "V:" + (brightness.ToString()+"%").PadRight(6,' ');
        }
    }
}
