using System.Drawing;
using System.Windows;
using System.Windows.Controls.Primitives;
using EmoteTool.ViewModels;
using Point = System.Drawing.Point;

namespace EmoteTool.Views
{
    public class DragThumb : Thumb
    {
        private readonly MainWindowViewModel _vm;
        private readonly DialogViewModel _dialogVm;

        public DragThumb()
        {
            _vm = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            _dialogVm = _vm.DialogViewModel;
            DragDelta += MoveThumb_DragDelta;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Image image = Image.FromFile(_vm.SelectedItem.ImagePath);

            var x = (int) (_dialogVm.DragPosition.X + e.HorizontalChange);

            var y = (int) (_dialogVm.DragPosition.Y + e.VerticalChange);

            if (x >= 100 || x < 0)
            {
                return;
            }
            if (y >= 100 || y < 0)
            {
                return;
            }

            _dialogVm.DragPosition = new Point(x, y);
        }
    }
}