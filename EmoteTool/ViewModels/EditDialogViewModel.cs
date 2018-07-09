using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using EmoteTool.Annotations;
namespace EmoteTool.ViewModels
{
    internal class EditDialogViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel MainViewModel { get; set; }
        public static string DefaultWatermark;
        public static Size ImagePreviewMaxSize = new Size(300, 300);

        private bool _isEditDialogOpen;
        private Point _dragPosition;
        private Size _dragSize;
        private string _watermarkName;

        public bool IsEditDialogOpen
        {
            get => _isEditDialogOpen;
            set
            {
                if (value == _isEditDialogOpen)
                {
                    return;
                }

                _isEditDialogOpen = value;
                MainViewModel.IsAnyDialogOpen = value;
                OnPropertyChanged();
            }
        }
        public Point DragPosition
        {
            get => _dragPosition;
            set
            {
                if (value == _dragPosition)
                {
                    return;
                }

                _dragPosition = value;
                OnPropertyChanged();
            }
        }
        public Size DragSize
        {
            get => _dragSize;
            set
            {
                if (value == _dragSize)
                {
                    return;
                }

                _dragSize = value;
                OnPropertyChanged();
            }
        }
        public string WatermarkName
        {
            get => _watermarkName;
            set
            {
                if (value == _watermarkName)
                {
                    return;
                }

                _watermarkName = value;
                OnPropertyChanged();
            }
        }
        public EditDialogViewModel()
        {
            DefaultWatermark = "Enter text for emote name";
            WatermarkName = DefaultWatermark;
        }

        public EditDialogViewModel(MainWindowViewModel viewModel)
        {
            MainViewModel = viewModel;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}