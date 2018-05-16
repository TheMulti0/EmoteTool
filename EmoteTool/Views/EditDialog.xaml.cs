using System.Windows;
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

            Window window = Application.Current.MainWindow;
            DataContext = window?.DataContext;
        }
    }
}