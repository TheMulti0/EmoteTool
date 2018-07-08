using System.Drawing;
using System.Windows;
using System.Windows.Controls.Primitives;
using EmoteTool.ViewModels;
using Point = System.Drawing.Point;

namespace EmoteTool.Views
{
    public class DragThumb : Thumb
    {
        private readonly MainWindowViewModel _mainVm;
        private readonly EditDialogViewModel _editVm;

        public DragThumb()
        {
            _mainVm = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            _editVm = _mainVm.EditDialogViewModel;
            DragDelta += MoveThumb_DragDelta;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Image image = Image.FromFile(_mainVm.SelectedItem.ImagePath);

            var x = (int) (_editVm.DragPosition.X + e.HorizontalChange);

            var y = (int) (_editVm.DragPosition.Y + e.VerticalChange);

            if (x >= 200 || x < 0)
            {
                return;
            }
            if (y >= 200 || y < 0)
            {
                return;
            }

            _editVm.DragPosition = new Point(x, y);
        }
    }
}