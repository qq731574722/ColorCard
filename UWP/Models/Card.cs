﻿using System;
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
        public static Card ShowingCard { get; set; }
        public static List<Card> ColorCards = new List<Card>();
    }
    class CardManager
    {
        public static List<Card> GetCards()
        {
            Card.ColorCards = new List<Card>();
            List<Color> colors1 = new List<Color>
            {
                new Color { RGB = "#177E89" },
                new Color { RGB = "#084C61" },
                new Color { RGB = "#FFC857" },
                new Color { RGB = "#DB3A34" }
            };
            List<Color> colors2 = new List<Color>
            {
                new Color { RGB = "Red" },
                new Color { RGB = "Blue" },
                new Color { RGB = "Green" },
                new Color { RGB = "Yellow" }
            };
            Card.ColorCards.Add(new Card { Name = "Colr", Colors = colors1, ColorNum = colors1.Count, ID = 8 });
            Card.ColorCards.Add(new Card { Name = "Card", Colors = colors1, ColorNum = colors1.Count, ID = 9 });
            Card.ColorCards.Add(new Card { Colors = colors1, ColorNum = colors1.Count, ID = 10 });
            Card.ColorCards.Add(new Card { Colors = colors2, ColorNum = colors1.Count, ID = 101 });
            Card.ColorCards.Add(new Card { Colors = colors2, ColorNum = colors1.Count, ID = 102 });
            Card.ColorCards.Add(new Card { Colors = colors2, ColorNum = colors1.Count, ID = 103 });
            return Card.ColorCards;
        }
    }
}
