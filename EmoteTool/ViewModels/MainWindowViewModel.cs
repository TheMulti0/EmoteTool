﻿using System;
using System.Collections.ObjectModel;
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

        public static EmoteItem SelectedItem { get; set; }

        public string EmoteName { get; set; }

        public Size IconSize { get; set; }

        private readonly string _emoteDefault;


        public MainWindowViewModel()
        {
            AddCommand = new Command(SelectImage);

            RemoveCommand = new Command(() => Emotes.Remove(SelectedItem));

            CopyCommand = new Command((item) =>
            {
                if (SelectedItem == null)
                {
                    SelectedItem = (EmoteItem) item;
                }
                Clipboard.SetImage(SelectedItem.Image);
            });

            Emotes = new ObservableCollection<EmoteItem>();

            _emoteDefault = "Enter an emote string here";
            EmoteName = _emoteDefault;

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
            if (dialog.ShowDialog() == true)
            {
                var image = Image.FromFile(dialog.FileName);
                var resized = ResizeBitmap(image, IconSize);
                var bitmapSource = BitmapToBitmapSource(resized);

                if (EmoteName == _emoteDefault || Emotes.Any(emote => EmoteName == emote.Name))
                {
                    var number = Emotes.Count + 1;
                    EmoteName = "Emote #" + number;
                }

                Emotes.Add(new EmoteItem(EmoteName, bitmapSource));
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