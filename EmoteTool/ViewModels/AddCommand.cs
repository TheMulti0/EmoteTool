using System;
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

        public AddCommand(MainWindowViewModel mainWindowViewModel)
        {
            _vm = mainWindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var value = parameter as string;
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
                if (_vm.EmoteName != _browsedItem.Name)
                {
                    _browsedItem.Name = _vm.EmoteName;
                }

                AcceptImage();
                return;
            }

            if (!ChooseFile(out string fileName))
            {
                return;
            }

            BitmapImage bitmapImage = SetUpImage(fileName);

            _vm.EmoteName = SortName();

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
                try
                {
                    Default.SavedEmotes.Add(item.Name + Seperator + _vm.FilePath);
                }
                catch
                {
                    Default.SavedEmotes.Add(item.Name + Seperator + item.ImagePath);
                }
            }
            catch
            {
                Default.SavedEmotes.Add(item.Name + Seperator + _browsedItem.ImagePath);
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
            return dialogChosen;
        }

        public string SortName(string name = "")
        {
            if (name == "")
            {
                name = _vm.EmoteName;
            }
            bool isInList = _vm.Emotes.Any(emote => name == emote.Name);
            if (!string.IsNullOrWhiteSpace(name)
                && !isInList
                && name != Seperator)
            {
                return name;
            }

            int number = _vm.Emotes.Count + 1;
            name = "Emote #" + number;
            return name;
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