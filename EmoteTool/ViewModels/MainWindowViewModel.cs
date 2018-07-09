using System;
using System.Collections;
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
        private ItemError _errorLabel;
        private bool _isAddDialogOpen;
        private bool _isAnyDialogOpen;
        private EmoteItem _selectedItem;
        private EmoteItem _newEmote;

        public static double FontSize { get; private set; }

        public AddCommand AddCommand { get; set; }

        public ICommand CopyCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand AddDialogCommand { get; set; }

        public ICommand EditDialogCommand { get; set; }

        public static string Seperator { get; private set; }

        public ObservableCollection<EmoteItem> Emotes { get; set; }

        public EmoteItem NewEmote
        {
            get => _newEmote;
            set
            {
                if (value == _newEmote)
                {
                    return;
                }
                _newEmote = value;
                OnPropertyChanged();
            }
        }

        public EmoteItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == _selectedItem)
                {
                    return;
                }
                if (value == null &&
                    _selectedItem != null)
                {
                    return;
                }

                _selectedItem = value;
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

        public ItemError ErrorLabel
        {
            get => _errorLabel;
            set
            {
                _errorLabel = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable ItemSizeModes { get; set; }

        public EditDialogViewModel EditDialogViewModel { get; set; }

        public MainWindowViewModel()
        {
            FontSize = 12;

            Seperator = ";;;;;;";

            AddCommand = new AddCommand(this);

            CopyCommand = new CopyCommand(this);

            RemoveCommand = new CommandFactory(RemoveImage);

            AddDialogCommand = new CommandFactory(() =>
            {
                IsAddDialogOpen = !IsAddDialogOpen;
            });

            EditDialogViewModel = new EditDialogViewModel(this);

            EditDialogCommand = new EditDialogCommand(this, EditDialogViewModel);

            Emotes = new ObservableCollection<EmoteItem>();

            IconSize = new Size((int) ItemSizeMode.Standard, (int) ItemSizeMode.Standard);

            ErrorLabel = ItemError.None;

            ItemSizeModes = Enum.GetValues(typeof(ItemSizeMode));

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
                string[] spitted = emote.Split(
                    new[]
                    {
                        Seperator
                    },
                    StringSplitOptions.None);

                string name = AddCommand.SortName(spitted[0]);
                string fileName = spitted[1];
                string sizeModeString = spitted[2];
                ItemSizeMode sizeMode;
                if (!Enum.TryParse(sizeModeString, out sizeMode))
                {
                    sizeMode = ItemSizeMode.Standard;
                }

                BitmapImage bitmapImage = AddCommand.SetUpImage(fileName);
                var emoteItem = new EmoteItem(name, bitmapImage, fileName, ItemSizeMode.Standard);

                Emotes.Add(emoteItem);
            }
        }

        private void RemoveImage(object item)
        {
            if (SelectedItem == null)
            {
                if (item != null)
                {
                    SelectedItem = (EmoteItem) item;
                }
                else if (Emotes.Count == 1)
                {
                    Emotes.Clear();
                    return;
                }
                else
                {
                    return;
                }
            }

            Emotes.Remove(SelectedItem);
            RemoveSelectedItemFromFile(SelectedItem?.Name);
        }

        public void RemoveSelectedItemFromFile(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = SelectedItem.Name;
            }

            List<string> list = Default.SavedEmotes
                .Cast<string>()
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