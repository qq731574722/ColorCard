using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWP.Models;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Color = Windows.UI.Color;
using Windows.Graphics.Imaging;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MinePage : INotifyPropertyChanged
    {

        WriteableBitmap image;
        private BitmapImage _image;
        public BitmapImage Image
        {
            set
            {
                _image = value;
                OnPropertyChanged();
            }
            get
            {
                return _image;
            }
        }

        private string color1;
        public string Color1
        {
            get { return color1; }
            set
            {
                if(color1!=value)
                {
                    color1 = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public MinePage()
        {
            this.InitializeComponent();
            PageLoaded();
            Card.ColorCards[0].Name = "色卡";
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName=null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = (UIElement)sender;
            double posX = Canvas.GetLeft(thumb) + e.HorizontalChange;
            double posY = Canvas.GetTop(thumb) + e.VerticalChange;
            Canvas.SetLeft(thumb, posX);
            Canvas.SetTop(thumb, posY);
            int pixelX = ConvertPosToPixel((int)posX + 12);
            int pixelY = ConvertPosToPixel((int)posY + 30);
            Color color = image.GetPixel(pixelX,pixelY);
            Color1 = color.ToString();
        }
        private async void PageLoaded()
        {
            Color1 = "#00FFFF";
            DataContext = this;
            var uri = new System.Uri("ms-appx:///Assets/Sample.jpg");
            _image = new BitmapImage();
            _image.UriSource = uri;
            img.Source = _image;
#pragma warning disable CS0618 // 类型或成员已过时
            image = await BitmapFactory.New(1, 1).FromContent(Image.UriSource); //把 BitmapImage 转 WriteableBitmapEx
#pragma warning restore CS0618 // 类型或成员已过时
            if (image.PixelWidth < PickAreaGrid.RenderSize.Width)
                img.Stretch = Stretch.None;
        }

        /* 计算缩放比将坐标转换为图片上的像素 */
        private int ConvertPosToPixel(double pos)
        {
            double scale = _image.PixelWidth / PickAreaGrid.RenderSize.Width;
            return (int)(pos*scale);
        }
    }
}
