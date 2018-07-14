using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using EmoteTool.Annotations;
using EmoteTool.Properties;
using Size = System.Drawing.Size;
using static EmoteTool.Program;

namespace EmoteTool.ViewModels
{
    internal class EmoteItem : INotifyPropertyChanged
    {
        private ItemSizeMode _sizeMode;
        private Size _itemSize;
        private Size _imageSize;
        private string _name;
        private string _imagePath;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public BitmapImage ResizedImage { get; set; }

        public string ActualImagePath { get; set; }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
                if (ActualImagePath.EndsWith(Name))
                {
                    return;
                }
                ActualImagePath = CopyImage(value);
            }
        }

        public ItemSizeMode SizeMode
        {
            get => _sizeMode;
            set
            {
                _sizeMode = value;
                SetSize();
            }
        }

        public Size ItemSize
        {
            get => _itemSize;
            set
            {
                _itemSize = value;
                OnPropertyChanged();
            }
        }

        public Size ImageSize
        {
            get => _imageSize;
            set
            {
                _imageSize = value;
                OnPropertyChanged();
            }
        }

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
            ActualImagePath = Path.Combine(
                ImagePath,
                Name);
            SizeMode = sizeMode;
            SetSize();
        }

        public static string CopyImage(string path)
        {
            string newPath = Path.Combine(
                ImagesPath,
                "Image1");
            File.Copy(path, newPath);

            return newPath;
        }

        public static string RenameImage(string newName, string oldName = "Image1")
        {
            string filePath = Path.Combine(
                ImagesPath,
                oldName);
            string newFilePath = filePath.Replace(oldName, newName);
            File.Move(filePath, newFilePath);
            return newFilePath;
        }

        private static double PointToPixels(double points)
        {
            PropertyInfo dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", BindingFlags.NonPublic | BindingFlags.Static);
            var dpiX = (int) dpiXProperty?.GetValue(null, null);

            return points * dpiX / 72;
        }

        private void SetSize()
        {
            ItemSize = new Size((int) SizeMode + 10, (int) SizeMode + 10);
            var fontPixels = (int) PointToPixels(MainWindowViewModel.NameFontSize);
            ImageSize = new Size((int) SizeMode - fontPixels, (int) SizeMode - fontPixels);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}