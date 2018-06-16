using System;
using System.Drawing;
using System.Windows.Input;

using static EmoteTool.Properties.Settings;

namespace EmoteTool.ViewModels
{
    internal class EditDialogCommand : ICommand
    {
        private readonly MainWindowViewModel _mainViewModel;
        private readonly EditDialogViewModel _vm;

        public event EventHandler CanExecuteChanged;

        public EditDialogCommand(MainWindowViewModel mainWindowViewModel,
                                 EditDialogViewModel viewModel)
        {
            _mainViewModel = mainWindowViewModel;
            _vm = viewModel;
        }

        public bool CanExecute(object parameter) 
            => true;

        public void Execute(object parameter)
        {
            if (_mainViewModel.SelectedItem == null)
            {
                return;
            }
            if (!_vm.IsEditDialogOpen)
            {
                _vm.IsEditDialogOpen = true;
                return;
            }
            _vm.IsEditDialogOpen = false;

            _mainViewModel.RemoveSelectedItemFromFile(_mainViewModel.SelectedItem.Name);
            _mainViewModel.SelectedItem.Name = _mainViewModel.AddCommand.SortName();

            SetNewItem();

            SetToDefault();
        }

        private void SetToDefault()
        {
            _mainViewModel.WatermarkName = MainWindowViewModel.DefaultWatermark;
            _mainViewModel.DragPosition = new Point(0, 0);
            _mainViewModel.DragSize = new Size(
                Math.Min((int)_mainViewModel.SelectedItem.ResizedImage.Width, 500),
                Math.Min((int)_mainViewModel.SelectedItem.ResizedImage.Height, 300));
        }

        private void SetNewItem()
        {
            var newItem = new EmoteItem(_mainViewModel.SelectedItem.Name,
                                        _mainViewModel.SelectedItem.ImagePath);

            int itemIndex = _mainViewModel.Emotes.IndexOf(_mainViewModel.SelectedItem);
            _mainViewModel.Emotes[itemIndex] = newItem;

            _mainViewModel.SelectedItem = _mainViewModel.Emotes[itemIndex];

            Default.SavedEmotes.Add(_mainViewModel.SelectedItem.Name +
                                    MainWindowViewModel.Seperator +
                                    _mainViewModel.SelectedItem.ImagePath);
        }
    }
}