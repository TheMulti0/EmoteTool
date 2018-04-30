using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using EmoteTool.Annotations;
using Microsoft.Win32;

using static EmoteTool.Properties.Settings;
using Size = System.Drawing.Size;

namespace EmoteTool.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly string _seperator;
        private EmoteItem _selectedItem;

        public ICommand AddCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand CopyCommand { get; set; }

        public ObservableCollection<EmoteItem> Emotes { get; set; }

        public EmoteItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == _selectedItem || value == null)
                {
                    return;
                }
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public string EmoteName { get; set; }

        public Size IconSize { get; set; }

        

        public MainWindowViewModel()
        {
            _seperator = ";;;;;;";

            AddCommand = new Command(SelectImage);

            RemoveCommand = new Command(() =>
            {
                Emotes.Remove(SelectedItem);

                List<string> list = Default.SavedEmotes.Cast<string>().ToList();
                string match = list.Find(s => s.StartsWith(SelectedItem.Name + _seperator));
                Default.SavedEmotes.Remove(match);
            });

            CopyCommand = new Command(CopyImage);

            Emotes = new ObservableCollection<EmoteItem>();

            IconSize = new Size(35, 35);

            ReadSavedEmotes();
        }

        private void ReadSavedEmotes()
        {
            if (Default.SavedEmotes == null)
            {
                Default.SavedEmotes = new StringCollection();
                return;
            }

            foreach (string emote in Default.SavedEmotes)
            {
                string[] splitted = emote.Split(_seperator.Split(' '), StringSplitOptions.None);
                string name = splitted[0];
                string fileName = splitted[1];

                BitmapSource bitmapSource = SetUpImage(fileName);
                var emoteItem = new EmoteItem(name, bitmapSource);

                Emotes.Add(emoteItem);
            }
        }

        public void SelectImage()
        {
            if (ChooseFile(out string fileName))
            {
                return;
            }

            BitmapSource bitmapSource = SetUpImage(fileName);

            SortName();

            var emoteItem = new EmoteItem(EmoteName, bitmapSource);
            Emotes.Add(emoteItem);

            Default.SavedEmotes.Add(EmoteName + _seperator + fileName);
        }

        private static bool ChooseFile(out string fileName)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png"
            };
            bool? nullDialogChosen = dialog.ShowDialog();

            fileName = dialog.FileName;

            bool dialogChosen = nullDialogChosen ?? false;
            return dialogChosen != true;
        }

        private BitmapSource SetUpImage(string fileName)
        {
            Image image = Image.FromFile(fileName);
            Bitmap resized = ImageToResizedBitmap(image, IconSize);
            BitmapSource bitmapSource = ImageToBitmapSource(resized);
            return bitmapSource;
        }

        private void SortName()
        {
            bool isInList = Emotes.Any(emote => EmoteName == emote.Name);
            if (!string.IsNullOrEmpty(EmoteName) 
                && !isInList 
                && EmoteName != _seperator)
            {
                return;
            }

            int number = Emotes.Count + 1;
            EmoteName = "Emote #" + number;
        }

        private static Bitmap ImageToResizedBitmap(Image imageToResize, Size size)
        {
            var bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(imageToResize, 0, 0, size.Width, size.Height);
            }
            return bitmap;
        }

        private static BitmapSource ImageToBitmapSource(Image bitmap)
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
                return;
            }
            if (SelectedItem != item)
            {
                SelectedItem = (EmoteItem) item;
                CopySelectedImage();
                return;
            }
            CopySelectedImage();

            void CopySelectedImage() 
                => Clipboard.SetImage(SelectedItem.Image);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}