using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using UWP.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWP
{
    /// <summary>
    /// 主页：色卡
    /// </summary>
    public sealed partial class HomePage : Page
    {
        //private List<Card> Cards;
        private ObservableCollection<Card> Cards;
        public HomePage()
        {
            this.InitializeComponent();
            Cards = new ObservableCollection<Card>();
            var mycards = Card.ColorCards;
            foreach (Card c in mycards)
            {
                Cards.Add(c);
            }
            switch (Card.Style)
            {
                case CardStyle.Bullseye:
                    GridView_Cards.ItemTemplate = (DataTemplate)this.Resources["CardTemplate_Bullseye"]; break;
                case CardStyle.Horizontal:
                    GridView_Cards.ItemTemplate = (DataTemplate)this.Resources["CardTemplate_Horizontal"]; break;
                case CardStyle.Vertical:
                    GridView_Cards.ItemTemplate = (DataTemplate)this.Resources["CardTemplate_Vertical"]; break;
            }
        }
        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            /*
            Card card = (Card)e.ClickedItem;
            ContentDialog nDialog = new ContentDialog()
            {
                Title = card.Name,
                Content = card.ID.ToString()+ Card.Style,
                CloseButtonText = "Ok",
            };
            if (string.IsNullOrEmpty(card.Name))
                nDialog.Title = "色卡 No." + card.ID;
            Cards.Remove(card);
            await nDialog.ShowAsync();
            */

            Card card = (Card)e.ClickedItem;
            Card.ShowingCard = card;
            //跳转至色卡详情界面
            Frame.Navigate(typeof(CardDetail));
            CardManager.SaveCardsAsync();
        }
    }
}
