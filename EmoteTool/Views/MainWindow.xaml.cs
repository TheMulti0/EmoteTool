using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using EmoteTool.Properties;
using EmoteTool.ViewModels;

namespace EmoteTool.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            _vm = (MainWindowViewModel) DataContext;

            Closing += OnClosing;
        }

        private static void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            Settings.Default.Save();
            //Settings.Default.Reload();
        }

    }
}