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

        private void CopyImage(object item)
        {
            try
            {
                if (SelectedItem == null)
                {
                    SelectedItem = (EmoteItem) item;
                }
                Clipboard.SetImage(SelectedItem.Image);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
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
            if (dialog.ShowDialog() == true)
            {
                Image image = Image.FromFile(dialog.FileName);
                Bitmap resized = ResizeBitmap(image, IconSize);
                BitmapSource bitmapSource = BitmapToBitmapSource(resized);

                string name = EmoteName;
                bool listContainsName = Emotes.Any(emote => EmoteName == emote.Name);
                if (string.IsNullOrEmpty(EmoteName) || listContainsName)
                {
                    int number = Emotes.Count + 1;
                    name = "Emote #" + number;
                }

                Emotes.Add(new EmoteItem(name, bitmapSource));
            }
        }

        public static Bitmap ResizeBitmap(Image imgToResize, Size size)
        {
            var b = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(b))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            }

            return b;
        }

        public BitmapSource BitmapToBitmapSource(Bitmap bitmap)
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
    }
}