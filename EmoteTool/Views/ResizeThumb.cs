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
        private readonly MainWindowViewModel _mainVm;
        private readonly EditDialogViewModel _editVm;
        private readonly Size _minSize;

        public ResizeThumb()
        {
            DragDelta += ResizeThumb_DragDelta;

            _mainVm = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            _editVm = _mainVm.EditDialogViewModel;
            _minSize = new Size(10, 10);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //double deltaVertical, deltaHorizontal;

            Size dragSize = _editVm.DragSize;
            switch (VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    deltaVertical = Math.Min(-e.VerticalChange, dragSize.Height - _minSize.Height);

                    _editVm.DragSize = new Size(dragSize.Width, Convert.ToInt32(dragSize.Height - deltaVertical));
                    break;
                case VerticalAlignment.Top:
                    deltaVertical = Math.Min(e.VerticalChange, dragSize.Height - _minSize.Height);

                    _editVm.DragPosition = new Point(
                        _editVm.DragPosition.X,
                        Convert.ToInt32(_editVm.DragPosition.Y + deltaVertical));

                    _editVm.DragSize = new Size(dragSize.Width, Convert.ToInt32(dragSize.Height - deltaVertical));
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    deltaHorizontal = Math.Min(e.HorizontalChange, dragSize.Width - _minSize.Width);
                    _editVm.DragPosition = new Point(
                        Convert.ToInt32(_editVm.DragPosition.X + deltaHorizontal),
                        _editVm.DragPosition.Y);
                    _editVm.DragSize = new Size(Convert.ToInt32(dragSize.Width - deltaHorizontal), dragSize.Height);
                    break;
                case HorizontalAlignment.Right:
                    deltaHorizontal = Math.Min(-e.HorizontalChange, dragSize.Width - _minSize.Width);

                    _editVm.DragSize = new Size(Convert.ToInt32(dragSize.Width - deltaHorizontal), dragSize.Height);
                    break;
                default:
                    break;
            }

            e.Handled = true;
        }
    }
}
