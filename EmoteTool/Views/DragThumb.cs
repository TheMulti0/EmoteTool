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
            _vm = Application.Current.MainWindow.DataContext as MainWindowViewModel;
            DragDelta += new DragDeltaEventHandler(MoveThumb_DragDelta);
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var image = System.Drawing.Image.FromFile(_vm.SelectedItem.ImagePath);
            
            int x = (int)(_vm.DragPosition.X + e.HorizontalChange);
            
            int y = (int)(_vm.DragPosition.Y + e.VerticalChange);
            
            if (x >= 200 ||
                x < 0)
            {
                return;
            }
            if (y >= 200 ||
                y < 0)
            {
                return;
            }
            
            
            _vm.DragPosition = new Point(x, y);

        }
    }
}