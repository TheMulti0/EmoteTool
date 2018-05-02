using System.Windows;
using MaterialDesignThemes.Wpf;

namespace EmoteTool.Views
{
    /// <summary>
    ///     Interaction logic for AddDialog.xaml
    /// </summary>
    public partial class AddDialog : DialogHost
    {
        public AddDialog()
        {
            InitializeComponent();

            var window = Application.Current.MainWindow;
            DataContext = window?.DataContext;
        }
    }
}