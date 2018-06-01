using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EmoteTool.Views
{
    internal class CommandGroup : DependencyObject, ICommand
    {

        public static readonly DependencyProperty CommandsProperty = DependencyProperty.Register(
            "Commands",
            typeof(ObservableCollection<ICommand>),
            typeof(CommandGroup));

        public ObservableCollection<ICommand> Commands
        {
            get => (ObservableCollection<ICommand>) GetValue(CommandsProperty);
            set
            {
                if (value == null)
                {
                    var commands = new ObservableCollection<ICommand>();
                    commands.CollectionChanged += OnCommandsCollectionChanged;
                    SetValue(CommandsProperty, commands);
                }

                SetValue(CommandsProperty, value);
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            foreach (ICommand command in Commands)
            {
                if (!command.CanExecute(parameter))
                {
                    return false;
                }
            }
            return true;
        }

        public void Execute(object parameter)
        {
            var taskFactory = new TaskFactory();
            foreach (ICommand command in Commands)
            {
                //taskFactory.StartNew(() => command.Execute(parameter));
                command.Execute(parameter);
            }
        }

        private void OnCommandsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCanExecuteChanged();

            IList newItems = e.NewItems;
            if (newItems != null &&
                newItems.Count > 0)
            {
                foreach (ICommand command in newItems)
                {
                    command.CanExecuteChanged += OnChildCommandCanExecuteChanged;
                }
            }

            IList oldItems = e.OldItems;
            if (oldItems == null ||
                0 >= oldItems.Count)
            {
                return;
            }

            foreach (ICommand command in oldItems)
            {
                command.CanExecuteChanged -= OnChildCommandCanExecuteChanged;
            }
        }

        private void OnChildCommandCanExecuteChanged(object sender, EventArgs e) 
            => OnCanExecuteChanged();

        protected virtual void OnCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    }
}