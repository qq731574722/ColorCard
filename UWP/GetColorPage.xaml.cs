using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP
{
    /// <summary>
    /// 取色
    /// </summary>
    public sealed partial class GetColorPage : INotifyPropertyChanged
    {
        private static WriteableBitmap image;
        private BitmapDecoder decoder;
        private byte[] colorData;
        //private BitmapImage bi;
        public  WriteableBitmap Image
        {
            set
            {
                image = value;
                OnPropertyChanged();
            }
            get
            {
                return image;
            }
        }
        private Card card;

        public GetColorPage()
        {
            this.InitializeComponent();
            card = CardManager.GetCards()[0];
            
            switch (Card.Style)
            {
                case CardStyle.Bullseye:
                    CardItem_Bullseye.Visibility = Visibility.Visible;
                    break;
                case CardStyle.Vertical:
                    CardItem_Vertical.Visibility = Visibility.Visible;
                    break;
                case CardStyle.Horizontal:
                    CardItem_Horizontal.Visibility = Visibility.Visible;
                    break;
            }
            Color0 = card.Colors[0].RGB;
            Color1 = card.Colors[1].RGB;
            Color2 = card.Colors[2].RGB;
            Color3 = card.Colors[3].RGB;
            DataContext = this;
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".bmp");
            var result = await filePicker.PickSingleFileAsync();

            if (result != null)
            {
                var stream = await result.OpenAsync(FileAccessMode.Read);
                decoder = await BitmapDecoder.CreateAsync(stream);
                image = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                var pixelData = await decoder.GetPixelDataAsync();
                image.SetSource(stream);
                Img.Source = image;
                colorData = pixelData.DetachPixelData();
                //System.Diagnostics.Debug.WriteLine(colorData[0].ToString());
                    
                /*  自适应图片大小 */
                if (image.PixelWidth <= 450)
                {
                    Img.MaxWidth = 450;
                    Img.Stretch = Stretch.Uniform;
                }
                else if (image.PixelWidth < PickAreaGrid.RenderSize.Width)
                {
                    Img.MaxWidth = int.MaxValue;
                    Img.Stretch = Stretch.None;
                }
                else
                {
                    Img.MaxWidth = int.MaxValue;
                    Img.Stretch = Stretch.Uniform;
                }
                //Color0 = GetPosRGB(Canvas.GetLeft(Picker0)+12, Canvas.GetTop(Picker0)+30);
                
                //Color0 = GetPosRGB(32.0, 50.0);
                //DragDeltaEventArgs dragDeltaEventArgs = new DragDeltaEventArgs(0, 0);
                //Thumb_DragDelta(Picker0, dragDeltaEventArgs);
                ReSelectImageButton.Visibility = Visibility.Visible;
                SelectImageButton.Visibility = Visibility.Collapsed;
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private string color0,color1,color2,color3;
        public string Color0
        {
            get { return color0; }
            set
            {
                if (color0 != value)
                {
                    color0 = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Color1
        {
            get { return color1; }
            set
            {
                if (color1 != value)
                {
                    color1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Color2
        {
            get { return color2; }
            set
            {
                if (color2 != value)
                {
                    color2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Color3
        {
            get { return color3; }
            set
            {
                if (color3 != value)
                {
                    color3 = value;
                    OnPropertyChanged();
                }
            }
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            card.Colors[0].RGB = Color0;
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = (UIElement)sender;
            double posX = Canvas.GetLeft(thumb) + e.HorizontalChange;
            double posY = Canvas.GetTop(thumb) + e.VerticalChange;
            if (posX >= 0 && posX <= PickAreaGrid.RenderSize.Width - 24)
                Canvas.SetLeft(thumb, posX);
            if (posY >= 0 && posY <= PickAreaGrid.RenderSize.Height - 30)
                Canvas.SetTop(thumb, posY);

            
            //int pixelX = ConvertPosToPixel((int)posX + 12);
            //int pixelY = ConvertPosToPixel((int)posY + 30);
            //var k = (pixelY * decoder.PixelWidth + pixelX) * 4;
            //Windows.UI.Color color = Windows.UI.Color.FromArgb(colorData[k + 3],colorData[k + 2], colorData[k + 1], colorData[k + 0]);
            ////Color0 = color.R.ToString() + color.G.ToString() + color.B.ToString();
            ////Color0 = Image.GetPixel(pixelX, pixelY).ToString();
            Color0 = GetPosRGB(posX+12, posY+30);
            
            //System.Diagnostics.Debug.WriteLine(Color0);
        }

        /* 计算缩放比将坐标转换为图片上的像素 */
        private int ConvertPosToPixel(double pos)
        {
            double scale = image.PixelWidth / PickAreaGrid.RenderSize.Width;
            return (int)(pos * scale);
        }
        private string ARGB2RGB(string ARGB)
        {
            string RGB = "#" + ARGB.Substring(3);
            return RGB;
        }
        private string GetPosRGB(double posX,double posY)
        {
            int pixelX = ConvertPosToPixel((int)posX);
            int pixelY = ConvertPosToPixel((int)posY);
            var k = (pixelY * decoder.PixelWidth + pixelX) * 4;
            Windows.UI.Color color=new Windows.UI.Color();
            if (k+4<=colorData.Length)
                color = Windows.UI.Color.FromArgb(colorData[k + 3], colorData[k + 2], colorData[k + 1], colorData[k + 0]);
            return ARGB2RGB(color.ToString());
        }
    }
}
