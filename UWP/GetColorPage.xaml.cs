using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class GetColorPage : Page
    {
        private Card card{ get; set; }
        public GetColorPage()
        {
            this.InitializeComponent();
            card = CardManager.GetCards()[0];

            switch (Card.Style)
            {
                case CardStyle.Bullseye:
                    CardItem.ContentTemplate = this.Resources["CardTemplate_Bullseye"] as DataTemplate;
                    break;
                case CardStyle.Vertical:
                    CardItem.ContentTemplate = this.Resources["CardTemplate_Vertical"] as DataTemplate;
                    break;
                case CardStyle.Horizontal:
                    CardItem.ContentTemplate = this.Resources["CardTemplate_Horizontal"] as DataTemplate;
                    break;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            FolderPicker pick = new FolderPicker();
            pick.FileTypeFilter.Add(".png");
            pick.FileTypeFilter.Add(".jpg");
            pick.FileTypeFilter.Add(".bmp");
            */
        }
    }
}
