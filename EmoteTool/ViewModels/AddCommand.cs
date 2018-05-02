using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

using static EmoteTool.Properties.Settings;
using static EmoteTool.ViewModels.MainWindowViewModel;

namespace EmoteTool.ViewModels
{
    internal class AddCommand : ICommand
    {
        private readonly MainWindowViewModel _vm;
        private EmoteItem _browsedItem;

        public event EventHandler CanExecuteChanged;

        public AddCommand(MainWindowViewModel mainWindowViewModel)
        {
            _vm = mainWindowViewModel;
        }

        public bool CanExecute(object parameter) 
            => true;

        public void Execute(object parameter)
        {
            string value = parameter as string;
            SelectImage(value);
        }

        public void SelectImage(string parameter = "")
        {
            if (parameter == "Accept")
            {
                if (string.IsNullOrWhiteSpace(_vm.FilePath)
                    || _browsedItem == null)
                {
                    return;
                }

                AcceptImage();
                return;
            }

            if (!ChooseFile(out string fileName))
            {
                return;
            }

            BitmapImage bitmapImage = SetUpImage(fileName);

            SortName();

            var emoteItem = new EmoteItem(_vm.EmoteName, bitmapImage);

            if (parameter == "Browse")
            {
                _browsedItem = new EmoteItem(_vm.EmoteName, bitmapImage);
                _vm.FilePath = fileName;
                return;
            }

            AddToCollections(emoteItem);
        }

        private void AcceptImage()
        {
            AddToCollections(_browsedItem);

            _vm.IsAddDialogOpen = false;
            _vm.EmoteName = "";
            _vm.FilePath = "";
            _browsedItem = null;
        }

        private void AddToCollections(EmoteItem item)
        {
            _vm.Emotes.Add(item);
            try
            {
                Default.SavedEmotes.Add(item.Name + Seperator + item.Image.UriSource.AbsolutePath);
            }
            catch (NullReferenceException)
            {
                try
                {
                    Default.SavedEmotes.Add(item.Name + Seperator + _browsedItem.Image.UriSource.AbsolutePath);
                }
                catch
                {
                    Default.SavedEmotes.Add(item.Name + Seperator + _vm.FilePath);
                }
            }
        }

        public BitmapImage SetUpImage(string fileName)
        {
            Image image = Image.FromFile(fileName);
            Bitmap resized = ImageToResizedBitmap(image, _vm.IconSize);
            BitmapImage bitmapImage = ImageToBitmapImage(resized);

            return bitmapImage;
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
            return dialogChosen == true;
        }

        private void SortName()
        {
            bool isInList = _vm.Emotes.Any(emote => _vm.EmoteName == emote.Name);
            if (!string.IsNullOrWhiteSpace(_vm.EmoteName) 
                && !isInList 
                && _vm.EmoteName != Seperator)
            {
                return;
            }

            int number = _vm.Emotes.Count + 1;
            _vm.EmoteName = "Emote #" + number;
        }

        private Bitmap ImageToResizedBitmap(Image imageToResize, Size size = default(Size))
        {
            if (size == default(Size))
            {
                size = _vm.IconSize;
            }

            var bitmap = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.DrawImage(imageToResize, 0, 0, size.Width, size.Height);
            }

            return bitmap;
        }

        private static BitmapImage ImageToBitmapImage(Image image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
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