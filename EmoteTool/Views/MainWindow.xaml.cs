using System.Windows;
using EmoteTool.ViewModels;

namespace EmoteTool.Views
{
    public partial class MainWindow : Window
    {
        public int height { get; set; } = 1000;
        public int width { get; set; } = 300;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}