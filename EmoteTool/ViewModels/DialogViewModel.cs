using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EmoteTool.Annotations;

namespace EmoteTool.ViewModels
{
    internal class DialogViewModel : INotifyPropertyChanged
    {
        public static string DefaultWatermark;
        public static Size ImagePreviewMaxSize;

        private string _watermarkName;
        private bool _isAnyDialogOpen;
        private bool _isAddDialogOpen;
        private bool _isEditDialogOpen;
        private Size _dragSize;
        private Point _dragPosition;

        public MainWindowViewModel MainViewModel { get; set; }

        public AddCommand AddCommand { get; set; }

        public ICommand AddDialogCommand { get; set; }

        public ICommand EditDialogCommand { get; set; }

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

        public bool IsAnyDialogOpen
        {
            get => _isAnyDialogOpen;
            set
            {
                if (value == _isAnyDialogOpen)
                {
                    return;
                }

                _isAnyDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public bool IsAddDialogOpen
        {
            get => _isAddDialogOpen;
            set
            {
                if (value == _isAddDialogOpen)
                {
                    return;
                }

                _isAddDialogOpen = value;
                IsAnyDialogOpen = value;
                OnPropertyChanged();
            }
        }

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
                IsAnyDialogOpen = value;
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

        public DialogViewModel()
        {
            Initialize();
        }

        public DialogViewModel(MainWindowViewModel viewModel)
        {
            MainViewModel = viewModel;
            Initialize();
        }

        private void Initialize()
        {
            ImagePreviewMaxSize = new Size(300, 300);
            DefaultWatermark = "Enter text for emote name";

            AddCommand = new AddCommand(MainViewModel);
            AddDialogCommand = new CommandFactory(() =>
            {
                IsAddDialogOpen = !IsAddDialogOpen;
            });
            EditDialogCommand = new EditDialogCommand(MainViewModel, this);

            WatermarkName = DefaultWatermark;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}