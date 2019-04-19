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
            if(e.Key==Windows.System.VirtualKey.Enter)
            {
                _card.Name = NewName.Text;
                Title.Text = NewName.Text;
                RenameArea.Visibility = Visibility.Collapsed;
                CardManager.SaveCard(_card.ID, _card.Name);
            }
        }
    }
}
