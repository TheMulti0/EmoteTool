using System.Windows;
using EmoteTool.ViewModels;
using MaterialDesignThemes.Wpf;

namespace EmoteTool.Views
{
    /// <summary>
    ///     Interaction logic for AddDialog.xaml
    /// </summary>
    public partial class EditDialog : DialogHost
    {
        public EditDialog()
        {
            InitializeComponent();

            var mainVm = App.Current.MainWindow.DataContext as MainWindowViewModel;
            DataContext = mainVm?.EditDialogViewModel;
        }
    }
}