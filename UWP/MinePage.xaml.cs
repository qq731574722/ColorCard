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

        private BitmapImage _image;
        WriteableBitmap image;
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
            Color1 = "#00FFFF";
            DataContext = this;
            var uri = new System.Uri("ms-appx:///Assets/partially-cloudy.png");
            _image=new BitmapImage();
            _image.UriSource = uri;
            img.Source = _image;
            BitmapImageConveter();
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
            /*
            if(image==null)
                image = await BitmapFactory.New(1, 1).FromContent(Image.UriSource); //我上面说的如何把 BitmapImage 转 WriteableBitmapEx
                */
            Color color = image.GetPixel((int)posX+12, (int)posY+30);

            Color1 = color.ToString();

        }
        private async void BitmapImageConveter()
        {
            image = await BitmapFactory.New(1, 1).FromContent(Image.UriSource); //我上面说的如何把 BitmapImage 转 WriteableBitmapEx
        }
    }
}
