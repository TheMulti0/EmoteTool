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
        private readonly DialogViewModel _dialogVm;
        private readonly Size _minSize;

        private Size _size;
        private double _horizontalDelta;
        private double _verticalDelta;

        public ResizeThumb()
        {
            DragDelta += ResizeThumb_DragDelta;

            var mainVm = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            _dialogVm = mainVm?.DialogViewModel;
            _minSize = new Size(10, 10);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            _size = _dialogVm.DragSize;
                
            (double width, double xPosition) = GetWidth(e.HorizontalChange);
            (double height, double yPosition) = GetHeight(e.VerticalChange);

            _dialogVm.DragSize = new Size((int) width, (int) height);
            _dialogVm.DragPosition = new Point((int) xPosition, (int) yPosition);
        }

        private (double width, double xPosition) GetWidth(double horizontalChange)
        {
            double width = _size.Width;
            double minWidth = _minSize.Width;
            double xPosition = _dialogVm.DragPosition.X;

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    _horizontalDelta = Math.Min(horizontalChange, width - minWidth);

                    xPosition += _horizontalDelta;
                    width -= _horizontalDelta;
                    break;
                case HorizontalAlignment.Right:
                    _horizontalDelta = Math.Min(-horizontalChange, width - minWidth);
                    width -= _horizontalDelta;
                    break;
            }
            return (width, xPosition);
        }

        private (double height, double yPosition) GetHeight(double verticalChange)
        {
            double height = _size.Height;
            double minHeight = _minSize.Height;
            double yPosition = _dialogVm.DragPosition.Y;

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    _verticalDelta = Math.Min(verticalChange, height - minHeight);

                    yPosition += _verticalDelta;
                    height -= _verticalDelta;
                    break;
                case VerticalAlignment.Bottom:
                    _verticalDelta = Math.Min(-verticalChange, height - minHeight);
                    height -= _verticalDelta;
                    break;
            }
            return (height, yPosition);
        }
    }
}