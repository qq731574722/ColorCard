using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UWP.Models
{
    //色卡的三种样式
    public enum CardStyle
    {
        Bullseye = 0, Vertical = 1, Horizontal = 2
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
        public static async void GetCardsAsync()
        {
            Card.ColorCards = new List<Card>();
            var localFloder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFloder.GetFileAsync("ColorCard.txt");
            IList<string> contents = await FileIO.ReadLinesAsync(file);

            for (int i = 0; i < contents.Count; i++)
            {
                string str = contents[i];
                if (str.Equals("[ColorCard]"))
                {
                    Card c = new Card();
                    c.ID = int.Parse(contents[i + 1]);
                    c.Name = contents[i + 2];
                    c.ColorNum = int.Parse(contents[i + 3]);
                    List<Color> colors = new List<Color>();
                    for (int j = i + 4; j < i + 4 + c.ColorNum; j++)
                    {
                        Color color = new Color { RGB = contents[j] };
                        colors.Add(color);
                    }
                    c.Colors = colors;
                    i += 3 + c.ColorNum;
                    Card.ColorCards.Add(c);
                }
            }
            /*
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
            */
        }
        public static async void SaveCardsAsync()
        {
            var localFloder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFloder.CreateFileAsync("ColorCard.txt", CreationCollisionOption.ReplaceExisting);

            var cards = Card.ColorCards;
            List<string> contents = new List<string>();
            foreach (Card card in cards)
            {
                contents.Add("[ColorCard]");
                contents.Add(card.ID.ToString());
                if (card.Name == null)
                    contents.Add("");
                else
                    contents.Add(card.Name);
                contents.Add(card.Colors.Count.ToString());
                foreach (Color color in card.Colors)
                {
                    contents.Add(color.RGB);
                }
            }
            await FileIO.WriteLinesAsync(file, contents);
        }
    }
}
