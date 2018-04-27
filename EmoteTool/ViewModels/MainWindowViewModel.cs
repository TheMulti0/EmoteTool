using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Size = System.Drawing.Size;

namespace EmoteTool.ViewModels
{
    internal class MainWindowViewModel
    {
        public ICommand AddCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand CopyCommand { get; set; }

        public ObservableCollection<EmoteItem> Emotes { get; set; }

        public EmoteItem SelectedItem { get; set; }

        public string EmoteName { get; set; }

        public Size IconSize { get; set; }



        public MainWindowViewModel()
        {
            AddCommand = new Command(SelectImage);

            RemoveCommand = new Command(() => Emotes.Remove(SelectedItem));

            CopyCommand = new Command(CopyImage);

            Emotes = new ObservableCollection<EmoteItem>();

            IconSize = new Size(35, 35);
        }

        public void SelectImage()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };
            if (dialog.ShowDialog() != true)
            {
                return;
            }

            Image image = Image.FromFile(dialog.FileName);
            Bitmap resized = ImageToResizedBitmap(image, IconSize);
            BitmapSource bitmapSource = BitmapToBitmapSource(resized);

            string name = SortEmptyName();

            Emotes.Add(new EmoteItem(name, bitmapSource));
        }

        private string SortEmptyName()
        {
            string name = EmoteName;
            bool listContainsName = Emotes.Any(emote => EmoteName == emote.Name);
            if (string.IsNullOrEmpty(EmoteName) || listContainsName)
            {
                int number = Emotes.Count + 1;
                name = "Emote #" + number;
            }

            return name;
        }

        private static Bitmap ImageToResizedBitmap(Image imageToResize, Size size)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imageToResize, 0, 0, size.Width, size.Height);
            }

            return bitmap;
        }

        private static BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;

                var result = new BitmapImage();

                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();

                return result;
            }
        }

        private void CopyImage(object item)
        {
            if (item == null)
            {
                CopySelectedImage();
            }
            else
            {
                if (SelectedItem != item)
                {
                    SelectedItem = (EmoteItem) item;
                    CopySelectedImage();
                }
                else
                {
                    CopySelectedImage();
                }
            }

            void CopySelectedImage() 
                => Clipboard.SetImage(SelectedItem.Image);
        }
    }
}