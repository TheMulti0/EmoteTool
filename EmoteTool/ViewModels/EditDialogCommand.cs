﻿using System;
using System.Drawing;
using System.Windows.Input;
using static EmoteTool.Program;

namespace EmoteTool.ViewModels
{
    internal class EditDialogCommand : ICommand
    {
        private readonly MainWindowViewModel _mainVm;
        private readonly DialogViewModel _dialogVm;

        public EditDialogCommand(
            MainWindowViewModel mainWindowViewModel,
            DialogViewModel dialogViewModel)
        {
            _mainVm = mainWindowViewModel;
            _dialogVm = dialogViewModel;
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
            if (!_dialogVm.IsEditDialogOpen)
            {
                _dialogVm.IsEditDialogOpen = true;
                return;
            }
            _dialogVm.IsEditDialogOpen = false;

            SetNewItem();
            SetToDefault();
        }

        private void SetToDefault()
        {
            _dialogVm.WatermarkName = DialogViewModel.DefaultWatermark;
            _dialogVm.DragPosition = new Point(0, 0);
            _dialogVm.DragSize = new Size(
                Math.Min((int)_mainVm.SelectedItem.ResizedImage.Width, 300),
                Math.Min((int)_mainVm.SelectedItem.ResizedImage.Height, 300));
        }

        private void SetNewItem()
        {
            EmoteItem selectedItem = _mainVm.SelectedItem;
            
            _mainVm.RemoveSelectedItemFromFile(selectedItem.Name);
            Settings.SavedEmotes.Add(
                (selectedItem.Name, selectedItem.ImagePath, selectedItem.SizeMode));
        }
    }
}