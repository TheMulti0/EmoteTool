using System.Drawing;
using System.Windows;
using System.Windows.Controls.Primitives;
using EmoteTool.ViewModels;
using Point = System.Drawing.Point;

namespace EmoteTool.Views
{
    public class DragThumb : Thumb
    {
        private readonly DialogViewModel _dialogVm;

        public DragThumb()
        {
            var vm = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            _dialogVm = vm?.DialogViewModel;

            DragDelta += MoveThumb_DragDelta;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double x = _dialogVm.DragPosition.X + e.HorizontalChange;
            double y = _dialogVm.DragPosition.Y + e.VerticalChange;

            if (x >= 100 || x < 0)
            {
                return;
            }
            if (y >= 100 || y < 0)
            {
                return;
            }
            _dialogVm.DragPosition = new Point((int) x, (int) y);
        }
    }
}