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

        public event EventHandler CanExecuteChanged;

        public AddCommand(MainWindowViewModel mainWindowViewModel)
        {
            _vm = mainWindowViewModel;
        }

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
            CheckError();
            if (_vm.ErrorLabel != AddError.None)
            {
                return;
            }
            if (HandleAcceptParameter(parameter))
            {
                return;
            }
            if (_vm.ErrorLabel != AddError.None)
            {
                return;
            }
            if (!ChooseFile(out string fileName))
            {
                return;
            }

            BitmapImage bitmapImage = SetUpImage(fileName);
            _vm.EmoteName = SortName();
            var item = new EmoteItem(_vm.EmoteName, bitmapImage, fileName);

            if (HandleBrowserParameter(parameter, item))
            {
                return;
            }

            AddToCollections(item);
        }

        private void CheckError()
        {
            bool isNameNull = string.IsNullOrWhiteSpace(_browsedItem?.Name);
            if (!isNameNull &&
                _browsedItem?.Image == null)
            {
                _vm.ErrorLabel = AddError.InvalidImage;
                return;
            }
            if (_browsedItem?.Name.Contains(Seperator) ?? false)
            {
                _vm.ErrorLabel = AddError.InvalidName;
                return;
            }
            _vm.ErrorLabel = AddError.None;
            return;
        }

        private bool HandleAcceptParameter(string parameter)
        {
            if (parameter != "Accept")
            {
                return false;
            }
            if (_browsedItem == null)
            {
                _vm.ErrorLabel = AddError.InvalidImage;
                return false;
            }
            if (string.IsNullOrWhiteSpace(_vm.FilePath)
                && string.IsNullOrWhiteSpace(_browsedItem.Name)
                && _browsedItem == null)
            {
                return true;
            }
            if (_vm.EmoteName != _browsedItem.Name)
            {
                _browsedItem.Name = _vm.EmoteName;
            }

            AcceptImage();
            return true;
        }

        private void AcceptImage()
        {
            AddToCollections(_browsedItem);

            _vm.IsAddDialogOpen = false;
            _vm.EmoteName = "";
            _vm.FilePath = "";
            _browsedItem = null;
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

        public BitmapImage SetUpImage(string fileName)
        {
            Image image = Image.FromFile(fileName);
            Bitmap resized = ImageToResizedBitmap(image, _vm.IconSize);
            BitmapImage bitmapImage = ImageToBitmapImage(resized);

            return bitmapImage;
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

        public string SortName(string name = "")
        {
            if (name == "")
            {
                name = _vm.EmoteName;
            }
            bool isInList = _vm.Emotes.Any(emote => name == emote.Name);
            if (!string.IsNullOrWhiteSpace(name)
                && name.StartsWith("Emote"))
            {
                char last = name.LastOrDefault();
                int i = int.Parse(last.ToString());
                return
                    i == _vm.Emotes.Count + 1
                        ? name
                        : HandleBadName();
            }
            if (!string.IsNullOrWhiteSpace(name)
                && !isInList
                && name != Seperator)
            {
                return name;
            }
            return HandleBadName();
        }

        private string HandleBadName()
        {
            int number = _vm.Emotes.Count + 1;
            string name = "Emote #" + number;

            return name;
        }

        private bool HandleBrowserParameter(string parameter, EmoteItem item)
        {
            if (parameter != "Browse")
            {
                return false;
            }

            _browsedItem = item;
            _vm.FilePath = item.ImagePath;
            return true;
        }

        private void AddToCollections(EmoteItem item)
        {
            _vm.Emotes.Add(item);
            if (!string.IsNullOrWhiteSpace(item?.ImagePath))
            {
                Default.SavedEmotes.Add(item.Name + Seperator + item.ImagePath);
                return;
            }
            if (item?.Image?.UriSource != null || item?.Image?.BaseUri != null)
            {
                Default.SavedEmotes.Add(
                    item.Name +
                    Seperator +
                    item.Image.UriSource?.AbsolutePath
                    ?? item.Image.BaseUri?.AbsolutePath);
                return;
            }
            if (_browsedItem != null)
            {
                if (!string.IsNullOrWhiteSpace(_browsedItem.ImagePath))
                {
                    Default.SavedEmotes.Add(
                        _browsedItem.Name + Seperator + _browsedItem.ImagePath);
                }
                if (_browsedItem.Image?.UriSource != null ||
                    _browsedItem.Image?.BaseUri != null)
                {
                    Default.SavedEmotes.Add(
                        _browsedItem.Name + Seperator + _browsedItem.Image.UriSource?.AbsolutePath
                        ?? _browsedItem.Image.BaseUri?.AbsolutePath);
                }
            }

            throw new NullReferenceException("Image path is empty.");
        }
    }
}