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
            if(Card.CardFrom==0)
            {
                Erase.Visibility = Visibility.Collapsed;
                if(_card.IsFavorite==1)
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
            if(e.Key==Windows.System.VirtualKey.Enter)
            {
                _card.Name = NewName.Text;
                Title.Text = NewName.Text;
                RenameArea.Visibility = Visibility.Collapsed;
                if (Card.CardFrom == 0)
                    CardManager.SaveCard(_card.ID, _card.Name);
                else if(Card.CardFrom == 1)
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
            foreach(Card c in Card.MyCards)
            {
                c.ID = cnt++;
            }
            CardManager.SaveMyCardsAsync();
            Erase.IsEnabled = false;
        }
    }
}
