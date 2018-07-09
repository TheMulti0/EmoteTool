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
        private readonly EditDialogViewModel _editVm;

        public DragThumb()
        {
            _vm = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            _editVm = _vm.EditDialogViewModel;
            DragDelta += MoveThumb_DragDelta;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Image image = Image.FromFile(_vm.SelectedItem.ImagePath);

            var x = (int) (_editVm.DragPosition.X + e.HorizontalChange);

            var y = (int) (_editVm.DragPosition.Y + e.VerticalChange);

            if (x >= 100 || x < 0)
            {
                return;
            }
            if (y >= 100 || y < 0)
            {
                return;
            }

            _editVm.DragPosition = new Point(x, y);
        }
    }
}