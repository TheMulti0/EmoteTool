using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace EmoteTool.ViewModels
{
    internal class MainWindowViewModel
    {
        public ICommand CopyCommand { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand RemoveCommand { get; set; }
        
        public BitmapImage ImageSource { get; set; }

        public Collection<BitmapImage> Images { get; set; }

        public BitmapImage SelectedItem { get; set; }

        public MainWindowViewModel()
        {
            CopyCommand = new Command(() => Clipboard.SetImage(SetSize(SelectedItem)));

            AddCommand = new Command(Open);

            RemoveCommand = new Command(() => Images.Remove(SelectedItem));

            ImageSource = new BitmapImage(new Uri("../Emoji.jpg", UriKind.Relative));

            Images = new ObservableCollection<BitmapImage>
            {
                ImageSource, ImageSource, ImageSource
            };
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
                var image = new BitmapImage(new Uri(op.FileName));
                Images.Add(image);
            }
        }

        public BitmapImage SetSize(BitmapImage image)
        {
            image.Clone();
            image.BeginInit();
            image.EndInit();
            return image;
        }
    }
}