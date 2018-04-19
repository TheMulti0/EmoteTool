using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Size = System.Drawing.Size;

namespace EmoteTool.ViewModels
{
    internal class MainWindowViewModel
    {
        public ICommand CopyCommand { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand RemoveCommand { get; set; }
        
        public BitmapImage ImageSource { get; set; }

        public ObservableCollection<BitmapSource> Images { get; set; }

        public BitmapSource SelectedItem { get; set; }

        public Size IconSize { get; set; }

        public MainWindowViewModel()
        {
            CopyCommand = new Command(() => Clipboard.SetImage(SelectedItem));

            AddCommand = new Command(Open);

            RemoveCommand = new Command(() => Images.Remove(SelectedItem));

            ImageSource = new BitmapImage(new Uri("../Emoji.jpg", UriKind.Relative));

            Images = new ObservableCollection<BitmapSource>
            {
                ImageSource, ImageSource, ImageSource
            };

            IconSize = new Size(35, 35);
        }

        public void Open()
        {
            var op = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == true)
            {
                Image image = Image.FromFile(op.FileName);
                Bitmap resized = ResizeImage(image, IconSize);
                BitmapSource bitmapSource = BitmapToBitmapSource(resized);
                Images.Add(bitmapSource);
            }
        }

        public static Bitmap ResizeImage(Image imgToResize, Size size)
        {
            try
            {
                var b = new Bitmap(size.Width, size.Height);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                }
                return b;
            }
            catch
            {
                return null;
            }
        }

        public BitmapSource BitmapToBitmapSource(Bitmap src)
        {
            var ms = new MemoryStream();
            src.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

    }
}