using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using EmoteTool.Annotations;
namespace EmoteTool.ViewModels
{
    internal class EditDialogViewModel : INotifyPropertyChanged
    {
        private bool _isEditDialogOpen;
        private Point _dragPosition;
        private Size _dragSize;
        public MainWindowViewModel MainViewModel { get; set; }

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
        public EditDialogViewModel()
        {
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