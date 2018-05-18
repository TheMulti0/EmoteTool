using EmoteTool.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

using Point = System.Drawing.Point;

namespace EmoteTool.Views
{
    public class DragThumb : Thumb
    {
        private MainWindowViewModel _vm;

        public DragThumb()
        {
            _vm = App.Current.MainWindow.DataContext as MainWindowViewModel;
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (_vm.DragPosition.X != null 
                && _vm.DragPosition.Y != null)
            {
                int x = Convert.ToInt32(_vm.DragPosition.X + e.HorizontalChange);
                int y = Convert.ToInt32(_vm.DragPosition.Y + e.VerticalChange);
                _vm.DragPosition = new Point(x, y);
            }
        }
    }
}