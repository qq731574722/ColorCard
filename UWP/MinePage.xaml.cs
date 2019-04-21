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
using System.Collections.ObjectModel;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MinePage:Page
    {
        //private List<Card> Cards;
        private ObservableCollection<Card> MyCards;
        public MinePage()
        {
            NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
            MyCards = new ObservableCollection<Card>();
            MyCards.Clear();
            var mycards = Card.MyCards;
            foreach(Card c in Card.ColorCards)
            {
                if(c.IsFavorite==1)
                {
                    MyCards.Add(c);
                }
            }
            foreach (Card c in mycards)
            {
                MyCards.Add(c);
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
            Card card = (Card)e.ClickedItem;
            Card.ShowingCard = card;
            if (card.IsFavorite == 1)
                Card.CardFrom = 0;
            else
                Card.CardFrom = 1;
            //跳转至色卡详情界面
            Frame.Navigate(typeof(CardDetail));
        }
    }
}
