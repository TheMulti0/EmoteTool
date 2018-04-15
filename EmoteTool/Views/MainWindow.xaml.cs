using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmoteTool.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            _vm = (MainWindowViewModel) DataContext;

        }
        

        public  void CopyCanvasToRTbitmap(Window window, Canvas canvas, int dpi)
        {
            var size = new Size(window.Width, window.Height);
            canvas.Measure(size); //canvas.Arrange(new Rect(size));

            var rtb = new RenderTargetBitmap(
                (int)_vm.WindowWidth,
                (int)_vm.WindowHeight, 
                dpi, //dpi X
                dpi, //dpi Y
                PixelFormats.Pbgra32 
            );
            rtb.Render(canvas);

            CopyRtbToClipboard(rtb);
        }
        private void CopyRtbToClipboard(BitmapSource bmp)
        {
            var croppedBmp = new CroppedBitmap(bmp, new Int32Rect(35,35,225,225));
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(croppedBmp));
            
            //convert rtb to BitmapImage
            var btmp = new BitmapImage();
            using (var stream = new MemoryStream())
            {
                enc.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                btmp.BeginInit();
                btmp.CacheOption = BitmapCacheOption.OnLoad;
                btmp.StreamSource = stream;
                btmp.EndInit();
            }

            Clipboard.SetImage(btmp);
        }
        
        //INPUT
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            CopyCanvasToRTbitmap(this, TestCanvas, 96);
        }
    }
}