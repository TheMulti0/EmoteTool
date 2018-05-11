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

            var window = Application.Current.MainWindow;
            DataContext = window?.DataContext;
        }
    }
}