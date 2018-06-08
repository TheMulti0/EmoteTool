using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace EmoteTool.ViewModels
{
    internal class EmoteItem
    {
        private string _imagePath;

        public string Name { get; set; }

        public BitmapImage ResizedImage { get; set; }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    return;
                }
                _imagePath = value;
            }
        }

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

        private static double TransformToPixels(double unit)
        {
            double pixel;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                pixel = (g.DpiX / 96 * unit);
            }

            return pixel;
        }

        private void ReplaceFromFile(EmoteItem item)
        {
            new MainWindowViewModel().RemoveSelectedItemFromFile(item.Name);
            new AddCommand().AddToCollections(item);
        }
    }
}