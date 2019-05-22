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
        public string RGB
        {
            get => hex;
            set
            {
                hex = value;
                R = Convert.ToInt32(hex.Substring(1, 2), 16);
                G = Convert.ToInt32(hex.Substring(3, 2), 16);
                B = Convert.ToInt32(hex.Substring(5, 2), 16);
            }
        }
        public string getTextRGB()
        {
            return "R:" + R.ToString().PadRight(6, ' ') + "G:" + G.ToString().PadRight(6, ' ') + "B:" + B.ToString().PadRight(6, ' ');
        }
        public string getTextHSV()
        {
            System.Drawing.Color color = System.Drawing.Color.FromArgb(R, G, B);
            int hue = (int)color.GetHue();
            int saturation = (int)(color.GetSaturation() * 100);
            int brightness = (int)(color.GetBrightness() * 100);
            return "H:" + hue.ToString().PadRight(6, ' ') + "S:" + (saturation.ToString() + "%").PadRight(6, ' ') + "V:" + (brightness.ToString() + "%").PadRight(6, ' ');
        }
    }
    class ColorManager
    {
        public static List<Windows.UI.Color> GetMainColor(List<Windows.UI.Color> colors, int k)
        {
            //等间距选取初始点
            var centroids = new List<Windows.UI.Color>();
            int gap = colors.Count / k;
            int cnt = 0;
            for (int i = 0; i < colors.Count && cnt < k; i += gap, cnt++)
                centroids.Add(colors[i]);
            //质心是否变化的标记
            bool isChanged = true;
            var cluster = new List<List<Windows.UI.Color>>();
            for (int i = 0; i < 4; i++)
                cluster.Add(new List<Windows.UI.Color>());
            //直到算法收敛
            while (isChanged)
            {
                foreach(var list in cluster)
                    list.Clear();
                //为每个点指派簇
                foreach (var color in colors)
                {
                    double minDistance = Double.MaxValue;
                    int ans = 0;
                    for (int i = 0; i < centroids.Count; i++)
                    {
                        var d = GetDistance(centroids[i], color);
                        if (d < minDistance)
                        {
                            minDistance = d;
                            ans = i;
                        }
                    }
                    cluster[ans].Add(color);
                }
                //重新计算质心
                isChanged = false;
                cnt = 0;
                foreach(var list in cluster)
                {
                    double R = 0, G = 0, B = 0;
                    foreach(var color in list)
                    {
                        R += color.R;
                        G += color.G;
                        B += color.B;
                    }
                    var new_center = new Windows.UI.Color();
                    new_center.A = 255;
                    new_center.R = (byte)(R / list.Count);
                    new_center.G = (byte)(G / list.Count);
                    new_center.B = (byte)(B / list.Count);
                    if (new_center != centroids[cnt])
                        isChanged = true;
                    centroids[cnt] = new_center;
                    cnt++;
                }
            }
            //确定质心后，选取离质心最近的点
            var res = new List<Windows.UI.Color>();
            cnt = 0;
            foreach(var list in cluster)
            {
                double minDistance = Double.MaxValue;
                var c = new Windows.UI.Color();
                foreach (var i in list)
                {
                    double d = GetDistance(centroids[cnt], i);
                    if(d<minDistance)
                    {
                        minDistance = d;
                        c = i;
                    }
                }
                res.Add(c);
                cnt++;
            }
            return res;
        }

        private static double GetDistance(Windows.UI.Color color1, Windows.UI.Color color2)
        {
            double res = 0;
            res += (color1.R - color2.R) * (color1.R - color2.R);
            res += (color1.G - color2.G) * (color1.G - color2.G);
            res += (color1.B - color2.B) * (color1.B - color2.B);
            return res;
        }
    }
}
