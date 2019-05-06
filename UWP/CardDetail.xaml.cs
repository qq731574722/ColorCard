using System;
using System.Collections.Generic;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications; // Notifications library
using Microsoft.QueryStringDotNET; // QueryString.NET
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Graphics.Display;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CardDetail : Page
    {
        private Card _card { get; set; }
        public CardDetail()
        {
            this.InitializeComponent();
            _card = Card.ShowingCard;
            if (_card.Name == null || _card.Name.Equals(""))
                Title.Text = "色卡";
            else
                Title.Text = _card.Name;
            NewName.Text = Title.Text;
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
            DataContext = this;
            if (Card.CardFrom == 0)
            {
                Erase.Visibility = Visibility.Collapsed;
                if (_card.IsFavorite == 1)
                {
                    Favorite.IsChecked = true;
                }
            }
            else
            {
                Favorite.Visibility = Visibility.Collapsed;
            }
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            if (RenameArea.Visibility == Visibility.Visible)
                RenameArea.Visibility = Visibility.Collapsed;
            else
            {
                RenameArea.Visibility = Visibility.Visible;
                NewName.Focus(FocusState.Keyboard);
                NewName.Select(0, NewName.Text.Length);
            }
        }

        private void Rename_Sure_Click(object sender, RoutedEventArgs e)
        {
            _card.Name = NewName.Text;
            Title.Text = NewName.Text;
            RenameArea.Visibility = Visibility.Collapsed;
            CardManager.SaveCard(_card.ID, _card.Name);
        }

        private void Button_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                _card.Name = NewName.Text;
                Title.Text = NewName.Text;
                RenameArea.Visibility = Visibility.Collapsed;
                if (Card.CardFrom == 0)
                    CardManager.SaveCard(_card.ID, _card.Name);
                else if (Card.CardFrom == 1)
                    CardManager.SaveMyCard(_card.ID, _card.Name);
            }
        }

        private void Favorite_Checked(object sender, RoutedEventArgs e)
        {
            Card.ColorCards[_card.ID].IsFavorite = 1;
            CardManager.SaveCardsAsync();
            Favorite.Icon = new SymbolIcon(Symbol.Favorite);
            Favorite.Label = "已收藏";
        }

        private void Favorite_Unchecked(object sender, RoutedEventArgs e)
        {
            Card.ColorCards[_card.ID].IsFavorite = 0;
            CardManager.SaveCardsAsync();
            Favorite.Icon = new SymbolIcon(Symbol.OutlineStar);
            Favorite.Label = "收藏";
        }

        private void Erase_Click(object sender, RoutedEventArgs e)
        {
            Card.MyCards.Remove(_card);
            int cnt = 0;
            foreach (Card c in Card.MyCards)
            {
                c.ID = cnt++;
            }
            CardManager.SaveMyCardsAsync();
            Erase.IsEnabled = false;
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var type = ((MenuFlyoutItem)sender).Tag.ToString();
            switch (type)
            {
                case "RGB":
                    t0.Text = _card.Colors[0].getTextRGB();
                    t1.Text = _card.Colors[1].getTextRGB();
                    t2.Text = _card.Colors[2].getTextRGB();
                    t3.Text = _card.Colors[3].getTextRGB();
                    ColorType.Label = "RGB";
                    break;
                case "HEX":
                    t0.Text = _card.Colors[0].RGB;
                    t1.Text = _card.Colors[1].RGB;
                    t2.Text = _card.Colors[2].RGB;
                    t3.Text = _card.Colors[3].RGB;
                    ColorType.Label = "HEX";
                    break;
                case "HSV":
                    t0.Text = _card.Colors[0].getTextHSV();
                    t1.Text = _card.Colors[1].getTextHSV();
                    t2.Text = _card.Colors[2].getTextHSV();
                    t3.Text = _card.Colors[3].getTextHSV();
                    ColorType.Label = "HSV";
                    break;
            }
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            //复制色卡内容到剪贴板
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            String text = "「色卡推荐」\n";
            foreach (Color color in _card.Colors)
            {
                text += (_card.Colors.IndexOf(color) + 1).ToString() + ".";
                text += color.RGB + "\n";
            }
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
            //构造通知内容
            string title = "色卡已复制到剪贴板";
            string content = text;
            content = content.Replace("「色卡推荐」\n", "");
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = title
                        },

                        new AdaptiveText()
                        {
                            Text = content
                        },

                    }
                }
            };
            int conversationId = 1;
            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                //Actions = actions,
                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "viewConversation" },
                    { "conversationId", conversationId.ToString() }

                }.ToString()
            };
            var toast = new ToastNotification(toastContent.GetXml());
            toast.ExpirationTime = DateTime.Now.AddMinutes(1);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private  async void Share_Click(object sender, RoutedEventArgs e)
        {
            var bitmap = new RenderTargetBitmap();
            string name;
            if (_card.Name == null || _card.Name == "")
                name = "色卡";
            else
                name = _card.Name;
            StorageFile file = await KnownFolders.PicturesLibrary.CreateFileAsync(name +".jpg",CreationCollisionOption.ReplaceExisting);
            await bitmap.RenderAsync(Stamp);
            var buffer = await bitmap.GetPixelsAsync();
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encod = await BitmapEncoder.CreateAsync(
                    BitmapEncoder.JpegEncoderId, stream);
                encod.SetPixelData(BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Ignore,
                    (uint)bitmap.PixelWidth,
                    (uint)bitmap.PixelHeight,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    DisplayInformation.GetForCurrentView().LogicalDpi,
                    buffer.ToArray()
                   );
                await encod.FlushAsync();
            }
            // In a real app, these would be initialized with actual data
            string title = "色卡已保存到图片库中";
            string image = file.Path;
            // Construct the visuals of the toast
            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = title
                        },
                        new AdaptiveImage()
                        {
                            Source = image
                        }
                    },
                }
            };
            int conversationId = 2;
            // Construct the actions for the toast (inputs and buttons)
            ToastActionsCustom actions = new ToastActionsCustom()
            {
                Buttons =
                {
                    new ToastButton("复制", new QueryString()
                    {
                        { "action", "copy" },
                        { "conversationId", conversationId.ToString() }

                    }.ToString())
                    /*
                    new ToastButton("查看", new QueryString()
                    {
                        { "action", "viewImage" },
                        { "imageUrl", image }

                    }.ToString())
                    */
                }
            };
            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
                Actions = actions,
                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "viewConversation" },
                    { "conversationId", conversationId.ToString() }

                }.ToString()
            };
            var toast = new ToastNotification(toastContent.GetXml());
            toast.ExpirationTime = DateTime.Now.AddMinutes(1);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromFile(file));
            Clipboard.SetContent(dataPackage);
        }
    }
}
