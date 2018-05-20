using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EmoteTool.ViewModels
{
    internal class EmoteItem
    {
        public string Name { get; set; }

        public BitmapImage Image { get; set; }

        public string ImagePath { get; set; }

        public EmoteItem(string name, BitmapImage image, string path = null)
        {
            Name = name;
            Image = image;
            
            ImagePath = path ?? image.UriSource?.AbsolutePath;
        }

        public EmoteItem(string name, string imagePath)
        {
            Name = name;
            ImagePath = imagePath;
            Image = new BitmapImage(new Uri(ImagePath));
        }
    }
}