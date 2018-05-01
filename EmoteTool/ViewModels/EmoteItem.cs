using System.Windows.Media.Imaging;

namespace EmoteTool.ViewModels
{
    internal class EmoteItem
    {
        public string Name { get; set; }

        public BitmapImage Image { get; set; }


        public EmoteItem(string name, BitmapImage image)
        {
            Name = name;
            Image = image;
        }
    }
}