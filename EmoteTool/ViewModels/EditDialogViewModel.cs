using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EmoteTool.Annotations;

using static EmoteTool.Properties.Settings;

namespace EmoteTool.ViewModels
{
    internal class EditDialogViewModel : INotifyPropertyChanged
    {
        private bool _isEditDialogOpen;

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