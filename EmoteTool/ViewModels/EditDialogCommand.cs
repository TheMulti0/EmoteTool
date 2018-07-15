using System;
using System.Drawing;
using System.IO;
using System.Windows.Input;
using static EmoteTool.Program;

namespace EmoteTool.ViewModels
{
    internal class EditDialogCommand : ICommand
    {
        private readonly MainWindowViewModel _vm;
        private readonly DialogViewModel _dialogVm;

        public EditDialogCommand(
            MainWindowViewModel mainWindowViewModel,
            DialogViewModel dialogViewModel)
        {
            _vm = mainWindowViewModel;
            _dialogVm = dialogViewModel;
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
            if (!_dialogVm.IsEditDialogOpen)
            {
                _dialogVm.IsEditDialogOpen = true;
                return;
            }
            _dialogVm.IsEditDialogOpen = false;

            SetToDefault();
            var item = _vm.SelectedItem;
            int index = Settings.SavedEmotes.FindIndex(t => t.id == item.Id);
            Settings.SavedEmotes[index] =
                (item.Id, item.Name, item.ImagePath, item.ActualImagePath, item.SizeMode);
        }

        private void SetToDefault()
        {
            _dialogVm.WatermarkName = DialogViewModel.DefaultWatermark;
            _dialogVm.DragPosition = new Point(0, 0);
            _dialogVm.DragSize = new Size(
                Math.Min((int)_vm.SelectedItem.ResizedImage.Width, 300),
                Math.Min((int)_vm.SelectedItem.ResizedImage.Height, 300));
        }
    }
}