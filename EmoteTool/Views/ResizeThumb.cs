using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using EmoteTool.ViewModels;

namespace EmoteTool.Views
{
    public class ResizeThumb : Thumb
    {
        private MainWindowViewModel _vm;

        public ResizeThumb()
        {
            _vm = App.Current.MainWindow.DataContext as MainWindowViewModel;
            DragDelta += ResizeThumb_DragDelta;
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var item = DataContext as Control;

            if (item != null)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange,
                            _vm.DragSize.Height - item.MinHeight);
                        item.Height -= deltaVertical;
                        break;
                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange,
                            _vm.DragSize.Height - item.MinHeight);
                        _vm.DragPosition = new System.Drawing.Point(
                           _vm.DragPosition.X,
                           Convert.ToInt32(_vm.DragPosition.Y + deltaVertical) );
                        item.Height -= deltaVertical;
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange,
                            _vm.DragSize.Width - item.MinWidth);
                        _vm.DragPosition = new System.Drawing.Point(
                            Convert.ToInt32(_vm.DragPosition.X + deltaHorizontal),
                            _vm.DragPosition.Y );
                        item.Width -= deltaHorizontal;
                        break;
                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange,
                            _vm.DragSize.Width - item.MinWidth);
                        item.Width -= deltaHorizontal;
                        break;
                }

            }
            e.Handled = true;
        }
    }
}