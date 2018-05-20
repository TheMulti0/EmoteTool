using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using EmoteTool.Annotations;
using static EmoteTool.Properties.Settings;

namespace EmoteTool.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private Point _dragPosition;
        private Size _dragSize;
        private string _emoteName;
        private string _filePath;
        private bool _isAddDialogOpen;
        private bool _isAnyDialogOpen;
        private bool _isEditDialogOpen;
        private EmoteItem _selectedItem;

        public AddCommand AddCommand { get; set; }

        public ICommand CopyCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand AddDialogCommand { get; set; }

        public ICommand EditDialogCommand { get; set; }

        public static string Seperator { get; private set; }

        public ObservableCollection<EmoteItem> Emotes { get; set; }

        public EmoteItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == _selectedItem)
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
                if (value == _filePath
                    || string.IsNullOrWhiteSpace(value))
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

        public Size IconSize { get; }

        public Point DragPosition
        {
            get => _dragPosition;
            set
            {
                if (value == _dragPosition)
                {
                    return;
                }
                _dragPosition = value;
                OnPropertyChanged();
            }
        }

        public Size DragSize
        {
            get => _dragSize;
            set
            {
                if (value == _dragSize)
                {
                    return;
                }
                _dragSize = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            Seperator = ";;;;;;";

            AddCommand = new AddCommand(this);

            CopyCommand = new CopyCommand(this);

            RemoveCommand = new CommandBase(RemoveImage);

            AddDialogCommand = new CommandBase(() =>
            {
                IsAddDialogOpen = !IsAddDialogOpen;
                FilePath = "";
            });

            EditDialogCommand = new CommandBase(EditDialog);

            Emotes = new ObservableCollection<EmoteItem>();

            IconSize = new Size(35, 35);

            ReadSavedEmotes();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ReadSavedEmotes()
        {
            if (Default.SavedEmotes == null)
            {
                Default.SavedEmotes = new StringCollection();
                return;
            }

            foreach (string emote in Default.SavedEmotes)
            {
                string[] splitted = emote.Split(new[] {Seperator},
                                                StringSplitOptions.None);
                string name = AddCommand.SortName(splitted[0]);
                string fileName = splitted[1];

                BitmapImage bitmapImage = AddCommand.SetUpImage(fileName);
                var emoteItem = new EmoteItem(name, bitmapImage, fileName);

                Emotes.Add(emoteItem);
            }
        }

        private void EditDialog()
        {
            if (SelectedItem == null)
            {
                return;
            }
            if (!IsEditDialogOpen)
            {
                IsEditDialogOpen = true;
                EmoteName = "";
                FilePath = SelectedItem.ImagePath;
                DragPosition = new Point(0, 0);
                DragSize = new Size(
                    Math.Min((int) (SelectedItem.Image.Width), 500),
                    Math.Min((int)(SelectedItem.Image.Height), 300));
            }
            else
            {
                IsEditDialogOpen = false;

                string oldName = SelectedItem.Name;
                RemoveSelectedItemFromFile(oldName);
                EmoteName = AddCommand.SortName();
                var newItem = new EmoteItem(EmoteName, FilePath ?? SelectedItem.ImagePath);
                int itemIndex = Emotes.IndexOf(SelectedItem);
                Emotes[itemIndex] = newItem;
                SelectedItem = Emotes[itemIndex];

                Default.SavedEmotes.Add(SelectedItem.Name + Seperator + FilePath);
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

            RemoveSelectedItemFromFile(EmoteName);
        }

        private void RemoveSelectedItemFromFile(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = SelectedItem.Name;
            }
            List<string> list = Default.SavedEmotes.Cast<string>()
                                       .ToList();
            string match = list.Find(s => s.StartsWith(name + Seperator));

            Default.SavedEmotes.Remove(match);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}