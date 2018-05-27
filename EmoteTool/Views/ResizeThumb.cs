using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using EmoteTool.ViewModels;

using Size = System.Drawing.Size;
using Point = System.Drawing.Point;

namespace EmoteTool.Views
{
    public class ResizeThumb : Thumb
    {
        private MainWindowViewModel _vm;
        private Size _minSize;

        public ResizeThumb()
        {
            _vm = App.Current.MainWindow.DataContext as MainWindowViewModel;
            _minSize = new Size(10, 10);
            DragDelta += ResizeThumb_DragDelta;
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double deltaVertical, deltaHorizontal;

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    deltaVertical = Math.Min(-e.VerticalChange,
                        _vm.DragSize.Height - _minSize.Height);

                    _vm.DragSize = new Size(_vm.DragSize.Width,
                        Convert.ToInt32(_vm.DragSize.Height - deltaVertical));
                    break;
                case VerticalAlignment.Top:
                    deltaVertical = Math.Min(e.VerticalChange,
                        _vm.DragSize.Height - _minSize.Height);
                    _vm.DragPosition = new Point(
                        _vm.DragPosition.X,
                        Convert.ToInt32(_vm.DragPosition.Y + deltaVertical));

                    _vm.DragSize = new Size(_vm.DragSize.Width,
                        Convert.ToInt32(_vm.DragSize.Height - deltaVertical));
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    deltaHorizontal = Math.Min(e.HorizontalChange,
                        _vm.DragSize.Width - _minSize.Width);
                    _vm.DragPosition = new Point(
                        Convert.ToInt32(_vm.DragPosition.X + deltaHorizontal),
                        _vm.DragPosition.Y);
                    _vm.DragSize = new Size(
                        Convert.ToInt32(_vm.DragSize.Width - deltaHorizontal),
                        _vm.DragSize.Height);
                    break;
                case HorizontalAlignment.Right:
                    deltaHorizontal = Math.Min(-e.HorizontalChange,
                        _vm.DragSize.Width - _minSize.Width);                   

                    _vm.DragSize = new Size(
                        Convert.ToInt32(_vm.DragSize.Width - deltaHorizontal),
                        _vm.DragSize.Height);
                    break;
            }

            e.Handled = true;
        }
    }
}