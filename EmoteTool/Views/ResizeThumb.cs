using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using EmoteTool.ViewModels;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace EmoteTool.Views
{
    public class ResizeThumb : Thumb
    {
        private readonly MainWindowViewModel _vm;
        private readonly Size _minSize;

        public ResizeThumb()
        {
            DragDelta += ResizeThumb_DragDelta;

            _vm = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            _minSize = new Size(10, 10);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double deltaVertical, deltaHorizontal;

            Size dragSize = _vm.DragSize;
            switch (VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    deltaVertical = Math.Min(-e.VerticalChange, dragSize.Height - _minSize.Height);

                    _vm.DragSize = new Size(dragSize.Width, Convert.ToInt32(dragSize.Height - deltaVertical));
                    break;
                case VerticalAlignment.Top:
                    deltaVertical = Math.Min(e.VerticalChange, dragSize.Height - _minSize.Height);
                    _vm.DragPosition = new Point(
                        _vm.DragPosition.X,
                        Convert.ToInt32(_vm.DragPosition.Y + deltaVertical));

                    _vm.DragSize = new Size(dragSize.Width, Convert.ToInt32(dragSize.Height - deltaVertical));
                    break;
                default:
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    deltaHorizontal = Math.Min(e.HorizontalChange, dragSize.Width - _minSize.Width);
                    _vm.DragPosition = new Point(
                        Convert.ToInt32(_vm.DragPosition.X + deltaHorizontal),
                        _vm.DragPosition.Y);
                    _vm.DragSize = new Size(Convert.ToInt32(dragSize.Width - deltaHorizontal), dragSize.Height);
                    break;
                case HorizontalAlignment.Right:
                    deltaHorizontal = Math.Min(-e.HorizontalChange, dragSize.Width - _minSize.Width);

                    _vm.DragSize = new Size(Convert.ToInt32(dragSize.Width - deltaHorizontal), dragSize.Height);
                    break;
                default:
                    break;
            }

            e.Handled = true;
        }
    }
}