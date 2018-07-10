using System;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Size = System.Drawing.Size;

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

        public EmoteItem()
        {
            
        }

        public EmoteItem(
            string name,
            BitmapImage resizedImage,
            string path = null,
            ItemSizeMode sizeMode = ItemSizeMode.Standard)
        {
            Name = name;
            ResizedImage = resizedImage;

            ImagePath = path ?? resizedImage.UriSource?.AbsolutePath;
            SizeMode = sizeMode;
            ItemSize = new Size((int) SizeMode + 10, (int) SizeMode + 10);
            var fontPixels = (int) PointToPixels(MainWindowViewModel.NameFontSize);
            ImageSize = new Size(
                (int) SizeMode - fontPixels,
                (int) SizeMode - fontPixels);
        }

        private static double PointToPixels(double points)
        {
            PropertyInfo dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiX = (int) dpiXProperty?.GetValue(null, null);

            return points * dpiX / 72;
        }
    }
}