using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
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
            try
            {
                await localFloder.GetFileAsync("ColorCard.txt");
            }
            catch (FileNotFoundException)
            {
                //若没有用户文件则从项目安装目录复制一份过去
                // TODO: 第一次打开绝对没有数据
                var src =await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/ColorCard.txt"));
                var dest = localFloder.Path + "\\ColorCard.txt";
                System.IO.File.Copy(src.Path, dest);
            }
            
            StorageFile file = await localFloder.GetFileAsync("ColorCard.txt");
            IList<string> contents = await FileIO.ReadLinesAsync(file);
            for (int i = 0; i < contents.Count; i++)
            {
                string str = contents[i];
                if (str.Equals("[ColorCard]"))
                {
                    Card c = new Card
                    {
                        ID = int.Parse(contents[i + 1]),
                        Name = contents[i + 2],
                        ColorNum = int.Parse(contents[i + 3])
                    };
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
        public static async Task InitFile()
        {

            var localFloder = ApplicationData.Current.LocalFolder;
            var src = await StorageFile.GetFileFromApplicationUriAsync(new System.Uri("ms-appx:///Assets/ColorCard.txt"));
            var dest = localFloder.Path + "\\ColorCard.txt";
            System.IO.File.Copy(src.Path, dest);
        }
    }
}
