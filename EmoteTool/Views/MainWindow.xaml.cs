using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using EmoteTool.Properties;
using EmoteTool.ViewModels;
using Newtonsoft.Json;
using static EmoteTool.Program;

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
            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(Program.Settings, Formatting.Indented));
        }
    }
}