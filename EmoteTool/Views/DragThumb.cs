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

        public DragThumb()
        {
            _vm = Application.Current.MainWindow?.DataContext as MainWindowViewModel;
            DragDelta += MoveThumb_DragDelta;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Image image = Image.FromFile(_vm.SelectedItem.ImagePath);

            var x = (int) (_vm.DragPosition.X + e.HorizontalChange);

            var y = (int) (_vm.DragPosition.Y + e.VerticalChange);

            if (x >= 200 || x < 0)
            {
                return;
            }
            if (y >= 200 || y < 0)
            {
                return;
            }


            _vm.DragPosition = new Point(x, y);
        }
    }
}