using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace EmoteTool.ViewModels
{
    internal class EmoteItem
    {
        public string Name { get; set; }

        public BitmapImage Image { get; set; }

        public string ImagePath { get; set; }

        public ItemSizeMode SizeMode { get; set; }

        public Size Size { get; set; }

        public EmoteItem(string name, BitmapImage image, string path = null, ItemSizeMode sizeMode = ItemSizeMode.Medium)
        {
            Name = name;
            Image = image;

            ImagePath = path ?? image.UriSource?.AbsolutePath;
            SizeMode = sizeMode;
            Size = new Size((int) SizeMode, (int) SizeMode);
        }

        public EmoteItem(string name, string imagePath, ItemSizeMode sizeMode = ItemSizeMode.Medium)
        {
            Name = name;
            ImagePath = imagePath;

            Image = new BitmapImage(new Uri(ImagePath ?? ""));
            SizeMode = sizeMode;
            Size = new Size((int)SizeMode, (int)SizeMode);
        }
    }
}