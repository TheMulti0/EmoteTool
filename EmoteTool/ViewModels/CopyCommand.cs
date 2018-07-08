using System;
using System.Windows;
using System.Windows.Input;

namespace EmoteTool.ViewModels
{
    internal class CopyCommand : ICommand
    {
        private readonly MainWindowViewModel _vm;

        public CopyCommand(MainWindowViewModel viewModel)
        {
            _vm = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_vm.SelectedItem == null)
            {
                return;
            }
            Clipboard.SetImage(_vm.SelectedItem.ResizedImage);
        }
    }
}