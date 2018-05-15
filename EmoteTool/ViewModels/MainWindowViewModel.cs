using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using EmoteTool.Annotations;
using Microsoft.Win32;

using static EmoteTool.Properties.Settings;
using Size = System.Drawing.Size;

namespace EmoteTool.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private EmoteItem _selectedItem;
        private bool _isAddDialogOpen;
        private string _filePath;
        private string _emoteName;
        private bool _isEditDialogOpen;
        private bool _isAnyDialogOpen;

        public AddCommand AddCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand CopyCommand { get; set; }

        public ICommand AddDialogCommand { get; set; }

        public ICommand EditDialogCommand { get; set; }

        public static string Seperator { get; private set; }

        public ObservableCollection<EmoteItem> Emotes { get; set; }

        public EmoteItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == _selectedItem || value == null)
                {
                    return;
                }
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        public string EmoteName
        {
            get => _emoteName;
            set
            {
                if (value == _emoteName)
                {
                    return;
                }

                _emoteName = value;
                OnPropertyChanged();
            }
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                if (value == _filePath)
                {
                    return;
                }

                _filePath = value;
                OnPropertyChanged();
            }
        }

        public bool IsAddDialogOpen
        {
            get => _isAddDialogOpen;
            set
            {
                if (value == _isAddDialogOpen)
                {
                    return;
                }

                _isAddDialogOpen = value;
                IsAnyDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditDialogOpen
        {
            get => _isEditDialogOpen;
            set
            {
                if (value == _isEditDialogOpen)
                {
                    return;
                }
                _isEditDialogOpen = value;
                IsAnyDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public bool IsAnyDialogOpen
        {
            get => _isAnyDialogOpen;
            set
            {
                if (value == _isAnyDialogOpen)
                {
                    return;
                }
                _isAnyDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public Size IconSize { get; private set; }

        public MainWindowViewModel()
        {
            Seperator = ";;;;;;";
            
            AddCommand = new AddCommand(this);
            
            RemoveCommand = new Command(RemoveImage);

            CopyCommand = new Command(CopyImage);

            AddDialogCommand = new Command(() =>
            {
                IsAddDialogOpen = !IsAddDialogOpen;
                FilePath = "";
            });

            EditDialogCommand = new Command(() =>
            {
                IsEditDialogOpen = !IsEditDialogOpen;
                FilePath = "";
            });

            Emotes = new ObservableCollection<EmoteItem>();

            IconSize = new Size(35, 35);

            ReadSavedEmotes();
        }

        private void ReadSavedEmotes()
        {
            if (Default.SavedEmotes == null)
            {
                Default.SavedEmotes = new StringCollection();
                return;
            }

            foreach (string emote in Default.SavedEmotes)
            {
                string[] splitted = emote.Split(MainWindowViewModel.Seperator.Split(' '),
                                                StringSplitOptions.None);
                string name = splitted[0];
                string fileName = splitted[1];

                BitmapImage bitmapImage = AddCommand.SetUpImage(fileName);
                var emoteItem = new EmoteItem(name, bitmapImage);

                Emotes.Add(emoteItem);
            }
        }

        private void RemoveImage(object item)
        {
            if (SelectedItem == null)
            {
                if (item != null)
                {
                    //SelectedItem = (EmoteItem)item;
                }
                if (Emotes.Count == 1)
                {
                    Emotes.Clear();
                    return;
                }
            }

            Emotes.Remove(SelectedItem);

            List<string> list = Default.SavedEmotes.Cast<string>().ToList();
            string match = list.Find(s => s.StartsWith(SelectedItem.Name + Seperator));

            Default.SavedEmotes.Remove(match);
        }

        private void CopyImage()
        {
            if (SelectedItem == null)
            {
                return;
            }
            Clipboard.SetImage(SelectedItem.Image);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}