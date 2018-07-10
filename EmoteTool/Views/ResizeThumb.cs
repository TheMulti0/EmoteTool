using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using EmoteTool.ViewModels;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace EmoteTool.Views
{
    class ResizeThumb : Thumb
    {
        private readonly EditDialogViewModel _editVm;
        private readonly Size _minSize;

        public ResizeThumb()
        {
            DragDelta += ResizeThumb_DragDelta;

            var _mainVm = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            _editVm = _mainVm.EditDialogViewModel;
            _minSize = new Size(10, 10);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double deltaV, deltaH;
            double vChange = e.VerticalChange;
            double hChange = e.HorizontalChange;
            Size size = _editVm.DragSize;

            double width = size.Width;
            double xPos = _editVm.DragPosition.X;
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    deltaH = Math.Min(hChange, width - _minSize.Width);
                    xPos += deltaH;
                    width -= deltaH;
                    break;
                case HorizontalAlignment.Right:
                    deltaH = Math.Min(-hChange, width - _minSize.Width);
                    width -= deltaH;
                    break;
            }

            double height = size.Height;
            double yPos = _editVm.DragPosition.Y;
            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    deltaV = Math.Min(vChange, height - _minSize.Height);
                    yPos += deltaV;
                    height -= deltaV;
                    break;
                case VerticalAlignment.Bottom:
                    deltaV = Math.Min(-vChange, height - _minSize.Height);
                    height -= deltaV;
                    break;
            }

            _editVm.DragSize = new Size((int)width, (int)height);
            _editVm.DragPosition = new Point((int)xPos, (int)yPos);
        }
    }
}