using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private List<Card> Cards;
        public HomePage()
        {
            this.InitializeComponent();
            Cards = CardManager.GetCards();
            Card.Style = CardStyle.Horizontal;
            switch(Card.Style)
            {
                case CardStyle.Bullseye:
                    GridView_Cards.ItemTemplate = (DataTemplate)this.Resources["CardTemplate_Bullseye"];   break;
                case CardStyle.Horizontal:
                    GridView_Cards.ItemTemplate = (DataTemplate)this.Resources["CardTemplate_Horizontal"]; break;
                case CardStyle.Vertical:
                    GridView_Cards.ItemTemplate = (DataTemplate)this.Resources["CardTemplate_Vertical"];   break;
            }
        }

        private async void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Card card = (Card)e.ClickedItem;
            ContentDialog nDialog = new ContentDialog()
            {
                Title = card.Name,
                Content = card.ID.ToString(),
                CloseButtonText = "Ok",
            };
            if (string.IsNullOrEmpty(card.Name))
                nDialog.Title = "色卡 No." + card.ID;
            await nDialog.ShowAsync();
        }
    }
}
