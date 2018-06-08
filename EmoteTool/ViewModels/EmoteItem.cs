using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace EmoteTool.ViewModels
{
    internal class EmoteItem
    {

        public string Name { get; set; }

        public BitmapImage ResizedImage { get; set; }

        public string ImagePath { get; set; }

        public ItemSizeMode SizeMode { get; set; }

        public Size ItemSize { get; set; }

        public Size ImageSize { get; set; }

        public EmoteItem(string name,
                         BitmapImage resizedImage,
                         string path = null,
                         ItemSizeMode sizeMode = ItemSizeMode.Standard)
        {
            Name = name;
            ResizedImage = resizedImage;

            ImagePath = path ?? resizedImage.UriSource?.AbsolutePath;
            SizeMode = sizeMode;
            ItemSize = new Size((int) SizeMode + 10, (int) SizeMode + 10);
            var fontPixels = (int) TransformToPixels(MainWindowViewModel.FontSize);
            ImageSize = new Size((int) SizeMode - fontPixels,
                            (int) SizeMode - fontPixels);
        }

        public EmoteItem(string name, string imagePath, ItemSizeMode sizeMode = ItemSizeMode.Standard)
        {
            Name = name;
            ImagePath = imagePath;

            ResizedImage = new BitmapImage(new Uri(ImagePath ?? ""));
            SizeMode = sizeMode;
            ImageSize = new Size((int) SizeMode, (int) SizeMode);
        }

        public double TransformToPixels(double unit)
        {
            double pixel;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                pixel = (g.DpiX / 72 * unit);
            }

            return pixel;
        }

    }
}