using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        private WriteableBitmap image;
        private BitmapDecoder decoder;

        private byte[] colorData;
        //private BitmapImage bi;
        public WriteableBitmap Image
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
            card = new Card();
            List<Color> colors = new List<Color>
            {
                new Color() { RGB = "#177E89" },
                new Color() { RGB = "#084C61" },
                new Color() { RGB = "#FFC857" },
                new Color() { RGB = "#DB3A34" }
            };
            card.Colors = colors;

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

                ReSelectImageButton.Visibility = Visibility.Visible;
                SelectImageButton.Visibility = Visibility.Collapsed;
                
                SaveCard.IsEnabled = true;
            }
        }

        private void Thumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            card.Colors[0].RGB = Color0;
            card.Colors[1].RGB = Color1;
            card.Colors[2].RGB = Color2;
            card.Colors[3].RGB = Color3;
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            UIElement thumb = (UIElement)sender;
            double posX = Canvas.GetLeft(thumb) + e.HorizontalChange;
            double posY = Canvas.GetTop(thumb) + e.VerticalChange;
            if (posX >= -11 && posX <= PickAreaGrid.RenderSize.Width - 12)
                Canvas.SetLeft(thumb, posX);
            if (posY >= -29 && posY <= PickAreaGrid.RenderSize.Height - 30)
                Canvas.SetTop(thumb, posY);
            if (thumb.Equals(Picker0))
                Color0 = GetPosRGB(posX + 12, posY + 30);
            if (thumb.Equals(Picker1))
                Color1 = GetPosRGB(posX + 12, posY + 30);
            if (thumb.Equals(Picker2))
                Color2 = GetPosRGB(posX + 12, posY + 30);
            if (thumb.Equals(Picker3))
                Color3 = GetPosRGB(posX + 12, posY + 30);
        }

        private void Img_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(PickAreaGrid.RenderSize.Width + "  " + image.PixelWidth);
            /*  自适应图片大小 */
            /*
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
            }*/
            Color0 = GetPickerRGB(Picker0);
            Color1 = GetPickerRGB(Picker1);
            Color2 = GetPickerRGB(Picker2);
            Color3 = GetPickerRGB(Picker3);
            PosChanged(Picker0, e);
            PosChanged(Picker1, e);
            PosChanged(Picker2, e);
            PosChanged(Picker3, e);
            if (AutoSelect.IsChecked == true)
            {
                /*
                DragDeltaEventArgs eventArgs = new DragDeltaEventArgs(random.Next(0,200), random.Next(0,200));
                Thumb_DragDelta(Picker0, eventArgs);
                */
                
                Random random = new Random();
                Canvas.SetTop(Picker0, random.Next(0, (int)PickAreaGrid.RenderSize.Height - 20));
                Canvas.SetLeft(Picker0, random.Next(0, (int)PickAreaGrid.RenderSize.Width-20));
                Canvas.SetTop(Picker1, random.Next(0, (int)PickAreaGrid.RenderSize.Height - 20));
                Canvas.SetLeft(Picker1, random.Next(0, (int)PickAreaGrid.RenderSize.Width - 20));
                Canvas.SetTop(Picker2, random.Next(0, (int)PickAreaGrid.RenderSize.Height - 20));
                Canvas.SetLeft(Picker2, random.Next(0, (int)PickAreaGrid.RenderSize.Width - 20));
                Canvas.SetTop(Picker3, random.Next(0, (int)PickAreaGrid.RenderSize.Height - 20));
                Canvas.SetLeft(Picker3, random.Next(0, (int)PickAreaGrid.RenderSize.Width - 20));
                Color0 = GetPickerRGB(Picker0);
                Color1 = GetPickerRGB(Picker1);
                Color2 = GetPickerRGB(Picker2);
                Color3 = GetPickerRGB(Picker3);
                
            }
        }

        private void PosChanged(Thumb picker, SizeChangedEventArgs e)
        {

            if (e.PreviousSize.Width == 0) return;
            double scaleX = e.NewSize.Width / e.PreviousSize.Width;
            double scaleY = e.NewSize.Height / e.PreviousSize.Height;
            double PreviousPosX = Canvas.GetLeft(picker);
            double PreviousPosY = Canvas.GetTop(picker);
            double newPosX = PreviousPosX * scaleX;
            double newPoxY = PreviousPosY * scaleY;
            DragDeltaEventArgs dragDeltaEventArgs = new DragDeltaEventArgs(newPosX - PreviousPosX, newPoxY - PreviousPosY);
            Thumb_DragDelta(picker, dragDeltaEventArgs);
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
        private string GetPosRGB(double posX, double posY)
        {
            int pixelX = ConvertPosToPixel((int)posX);
            int pixelY = ConvertPosToPixel((int)posY);
            var k = (pixelY * decoder.PixelWidth + pixelX) * 4;
            Windows.UI.Color color = new Windows.UI.Color();
            if (k >= 0 && k + 3 < colorData.Length)
                color = Windows.UI.Color.FromArgb(colorData[k + 3], colorData[k + 2], colorData[k + 1], colorData[k + 0]);
            return ARGB2RGB(color.ToString());
        }
        private string GetPickerRGB(Thumb picker) => GetPosRGB(Canvas.GetLeft(picker) + 12, Canvas.GetTop(picker) + 30);

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string color0, color1, color2, color3;
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

        private void SaveCard_Click(object sender, RoutedEventArgs e)
        {
            card.IsFavorite = 0;
            card.Name = "";
            card.ID = Card.MyCards.Count;
            card.Colors = new List<Color>();
            card.Colors.Add(new Color() { RGB = Color0 });
            card.Colors.Add(new Color() { RGB = Color1 });
            card.Colors.Add(new Color() { RGB = Color2 });
            card.Colors.Add(new Color() { RGB = Color3 });
            Card.MyCards.Add(card);
            CardManager.SaveMyCardsAsync();
            SaveCard.IsEnabled = false;
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

    }
}
