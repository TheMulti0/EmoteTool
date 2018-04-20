using System.Windows.Media.Imaging;

namespace EmoteTool.ViewModels
{
    internal class EmoteItem
    {
        public string Name { get; set; }

        public BitmapSource Image { get; set; }


        public EmoteItem(string name, BitmapSource image)
        {
            Name = name;
            Image = image;
        }
    }
}