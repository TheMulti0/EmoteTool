using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using EmoteTool.Annotations;
using Size = System.Drawing.Size;

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

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
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
            SizeMode = sizeMode;
            SetSize();
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