using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace EmoteTool.ViewModels
{
    internal class MainWindowViewModel
    {
        public ICommand CopyCommand { get; set; }

        public ICommand AddCommand { get; set; }

        public ICommand OpenCommand { get; set; }

        public BitmapImage ImageSource { get; set; }

        public Collection<BitmapImage> Images { get; set; }

        public BitmapImage SelectedItem { get; set; }

        public MainWindowViewModel()
        {
            CopyCommand = new Command(() => Clipboard.SetImage(SelectedItem));

            AddCommand = new Command(image => Images.Add((BitmapImage) image));

            OpenCommand = new Command(Open);

            ImageSource = new BitmapImage(new Uri("../Emoji.jpg", UriKind.Relative));

            Images = new ObservableCollection<BitmapImage>
            {
                ImageSource
            };
        }

        public void Open()
        {
            BitmapImage img = null;
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select an image",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == true)
            {
                img = new BitmapImage(new Uri(op.FileName));
                AddCommand.Execute(img);
            }
        }
    }
}