using System.Windows;
using EmoteTool.ViewModels;
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

            var mainVm = App.Current.MainWindow.DataContext as MainWindowViewModel;
            DataContext = mainVm?.DialogViewModel;
        }
    }
}