using System;
using System.Drawing;
using System.Windows.Input;
using static EmoteTool.Properties.Settings;

namespace EmoteTool.ViewModels
{
    internal class EditDialogCommand : ICommand
    {
        private readonly MainWindowViewModel _mainVm;
        private readonly EditDialogViewModel _editVm;

        public EditDialogCommand(
            MainWindowViewModel mainWindowViewModel,
            EditDialogViewModel editDialogViewModel)
        {
            _mainVm = mainWindowViewModel;
            _editVm = editDialogViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_mainVm.SelectedItem == null)
            {
                return;
            }
            if (!_editVm.IsEditDialogOpen)
            {
                _editVm.IsEditDialogOpen = true;
                return;
            }
            _editVm.IsEditDialogOpen = false;

            _mainVm.RemoveSelectedItemFromFile(_mainVm.SelectedItem.Name);
            _mainVm.SelectedItem.Name = _mainVm.AddCommand.SortName();

            SetNewItem();

            SetToDefault();
        }

        private void SetToDefault()
        {
            _editVm.WatermarkName = EditDialogViewModel.DefaultWatermark;
            _editVm.DragPosition = new Point(0, 0);
            _editVm.DragSize = new Size(
                Math.Min((int)_mainVm.SelectedItem.ResizedImage.Width, 300),
                Math.Min((int)_mainVm.SelectedItem.ResizedImage.Height, 300));
        }

        private void SetNewItem()
        {
            var newItem = new EmoteItem(
                _mainVm.SelectedItem.Name,
                _mainVm.SelectedItem.ImagePath);

            int itemIndex = _mainVm.Emotes.IndexOf(_mainVm.SelectedItem);
            _mainVm.Emotes[itemIndex] = newItem;

            _mainVm.SelectedItem = _mainVm.Emotes[itemIndex];

            Default.SavedEmotes.Add(
                _mainVm.SelectedItem.Name +
                MainWindowViewModel.Seperator +
                _mainVm.SelectedItem.ImagePath);
        }
    }
}