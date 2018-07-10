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
        
        private EmoteItem _selectedItem;
        private EmoteItem _newEmote;
        private ItemError _errorLabel;

        public static double NameFontSize { get; private set; }

        public ICommand CopyCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

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
                if (value == null &&
                    _selectedItem != null)
                {
                    return;
                }

                _selectedItem = value;
                OnPropertyChanged();
            }
        }

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

        public ItemError ErrorLabel
        {
            get => _errorLabel;
            set
            {
                _errorLabel = value;
                OnPropertyChanged();
            }
        }

        public Size IconSize { get; }

        public IEnumerable ItemSizeModes { get; set; }

        public DialogViewModel DialogViewModel { get; set; }

        public MainWindowViewModel()
        {
            NameFontSize = 12;
            Seperator = ";;;;;;";

            CopyCommand = new CopyCommand(this);
            RemoveCommand = new CommandFactory(RemoveImage);
                
            DialogViewModel = new DialogViewModel(this);

            Emotes = new ObservableCollection<EmoteItem>();
            NewEmote = new EmoteItem();

            IconSize = new Size((int) ItemSizeMode.Standard, (int) ItemSizeMode.Standard);
            ErrorLabel = ItemError.None;
            ItemSizeModes = Enum.GetValues(typeof(ItemSizeMode));

            ReadSavedEmotes();
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

                string name = DialogViewModel.AddCommand.SortName(spitted[0]);
                string fileName = spitted[1];
                string sizeModeString = spitted[2];
                ItemSizeMode sizeMode;
                if (!Enum.TryParse(sizeModeString, out sizeMode))
                {
                    sizeMode = ItemSizeMode.Standard;
                }

                BitmapImage bitmapImage = DialogViewModel.AddCommand.SetUpImage(fileName);
                var emoteItem = new EmoteItem(name, bitmapImage, fileName, sizeMode);

                Emotes.Add(emoteItem);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}