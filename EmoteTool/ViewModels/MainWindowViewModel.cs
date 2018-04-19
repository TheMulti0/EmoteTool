using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EmoteTool.ViewModels
{
    internal class MainWindowViewModel
    {
        public ICommand ButtonCommand { get; set; }

        public BitmapImage ImageSource { get; set; }

        public MainWindowViewModel()
        {
            ButtonCommand = new Command(() => Clipboard.SetImage(ImageSource));

            ImageSource = new BitmapImage(new Uri("../Emoji.jpg", UriKind.Relative));
        }
    }
}