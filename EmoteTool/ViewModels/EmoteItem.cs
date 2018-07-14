using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private string _actualImagePath;

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
            string path,
            BitmapImage resizedImage = null,
            ItemSizeMode sizeMode = ItemSizeMode.Standard)
        {
            Name = name;
            ResizedImage = resizedImage ?? new BitmapImage(new Uri(path));

            ActualImagePath = Path.Combine(ImagesPath, Name, Path.GetExtension(path));
            ImagePath = path;
            
            SizeMode = sizeMode;
            SetSize();
        }

        public static string CopyImage(string path)
        {
            if (!Directory.Exists(ImagesPath))
            {
                Directory.CreateDirectory(ImagesPath);
            }
            string extention = Path.GetExtension(path);
            string newPath = Path.Combine(ImagesPath, $"Image1{extention}");
            if (File.Exists(newPath))
            {
                File.Delete(newPath);
            }
            File.Copy(path, newPath);

            return newPath;
        }

        public static string RenameImage(string newName, string oldPath)
        {
            string newFilePath = oldPath.Replace(Path.GetFileNameWithoutExtension(oldPath), newName);
            File.Copy(oldPath, newFilePath, true);
            File.Delete(oldPath);
            
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