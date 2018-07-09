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
            double deltaV, deltaH;
            double VChange = e.VerticalChange;
            double HChange = e.HorizontalChange;

            Size size = _editVm.DragSize;
            double WToChangeTo = size.Width;
            double HToChangeTo = size.Height;
            double XToChangeTo = _editVm.DragPosition.X;
            double YToChangeTo = _editVm.DragPosition.Y;

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                    deltaV = Math.Min(VChange, size.Height - _minSize.Height);

                    YToChangeTo += deltaV;
                    HToChangeTo -= deltaV;
                    break;
                case VerticalAlignment.Bottom:
                    deltaV = Math.Min(-VChange, size.Height - _minSize.Height);
                    HToChangeTo -= deltaV;
                    break;
            }

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    deltaH = Math.Min(HChange, size.Width - _minSize.Width);

                    XToChangeTo += deltaH;
                    WToChangeTo -= deltaH;
                    break;
                case HorizontalAlignment.Right:
                    deltaH = Math.Min(-HChange, size.Width - _minSize.Width);
                    WToChangeTo -= deltaH;
                    break;
            }

            _editVm.DragSize = new Size((int)WToChangeTo, (int)HToChangeTo);
            _editVm.DragPosition = new Point((int)XToChangeTo, (int)YToChangeTo);
        }
    }
}




//e.Handled = true;  ?