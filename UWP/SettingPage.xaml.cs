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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            this.InitializeComponent();
            if (Card.Style == CardStyle.Bullseye)
                MyGridView.SelectedItem = CardTemplate_Bullseye;
            else if (Card.Style == CardStyle.Horizontal)
                MyGridView.SelectedItem = CardTemplate_Horizontal;
            else if (Card.Style == CardStyle.Vertical)
                MyGridView.SelectedItem = CardTemplate_Vertical;
        }

        private void MyGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CardTemplate_Bullseye.IsSelected)
            {
                Card.Style = CardStyle.Bullseye;
            }
            else if (CardTemplate_Horizontal.IsSelected)
            {
                Card.Style = CardStyle.Horizontal;
            }
            else if (CardTemplate_Vertical.IsSelected)
            {
                Card.Style = CardStyle.Vertical;
            }
        }
    }
}
