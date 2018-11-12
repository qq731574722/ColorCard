using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.Models
{
    //色卡的三种样式
    public enum CardStyle
    {   
        Bullseye=0,Vertical=1,Horizontal=2
    }
    class Card
    {
        public String Name { get; set; }
        public List<Color> Colors { get; set; }
        public int ColorNum { get; set; }
        public int ID { get; set; }
        public static CardStyle Style { get; set; }
    }

    class CardManager
    {
        public static List<Card> GetCards()
        {
            List<Card> cards = new List<Card>();
            List<Color> colors1 = new List<Color>
            {
                new Color { ARGB = "#FF177E89" },
                new Color { ARGB = "#FF084C61" },
                new Color { ARGB = "#FFFFC857" },
                new Color { ARGB = "#FFDB3A34" }
            };
            List<Color> colors2 = new List<Color>
            {
                new Color { ARGB = "Red" },
                new Color { ARGB = "Blue" },
                new Color { ARGB = "Green" },
                new Color { ARGB = "Yellow" }
            };
            cards.Add(new Card { Name = "Colr", Colors = colors1, ColorNum = colors1.Count, ID = 8 });
            cards.Add(new Card { Name = "Card", Colors = colors1, ColorNum = colors1.Count, ID = 9 });
            cards.Add(new Card { Colors = colors1, ColorNum = colors1.Count, ID = 10 });
            cards.Add(new Card { Colors = colors2, ColorNum = colors1.Count, ID = 101 });
            cards.Add(new Card { Colors = colors2, ColorNum = colors1.Count, ID = 102 });
            cards.Add(new Card { Colors = colors2, ColorNum = colors1.Count, ID = 103 });
            return cards;
        }
    }
}
