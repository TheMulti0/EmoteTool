using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EmoteTool.ViewModels
{
    internal class CopyCommand : ICommand
    {
        private readonly MainWindowViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public CopyCommand(MainWindowViewModel viewModel)
        {
            _vm = viewModel;
        }

        public bool CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
        {
            if (_vm.SelectedItem == null)
            {
                return;
            }
            Clipboard.SetImage(SelectedItem.Image);
        }
    }
}
