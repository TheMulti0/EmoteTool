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

        public AddCommand AddCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand CopyCommand { get; set; }

        public ICommand AddDialogCommand { get; set; }

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

        public string EmoteName { get; set; }

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

            AddDialogCommand = new Command(() => IsAddDialogOpen = !IsAddDialogOpen);

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

        private void RemoveImage()
        {
            if (SelectedItem == null)
            {
                try
                {
                    EmoteItem item = Emotes.FirstOrDefault(emote => emote.Name == EmoteName);
                    RemoveEmote(item);
                }
                catch (Exception)
                {
                }

                IsAddDialogOpen = false;
                return; 
            }

            RemoveEmote(SelectedItem);
        }

        private void RemoveEmote(EmoteItem item)
        {
            Emotes.Remove(item);

            try
            {
                List<string> list = Default.SavedEmotes.Cast<string>().ToList();
                string match = list.Find(s => s.StartsWith(SelectedItem.Name + Seperator));

                Default.SavedEmotes.Remove(match);
            }
            catch (NullReferenceException)
            {
                Default.SavedEmotes.Remove(
                    item.Name 
                    + Seperator 
                    + item.Image.BaseUri.AbsolutePath);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error adding to saved file " + e);
                throw;
            }
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