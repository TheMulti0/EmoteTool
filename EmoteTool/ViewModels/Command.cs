using System;
using System.Windows.Input;

namespace EmoteTool.ViewModels
{
    internal class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;


        public Action Action { get; set; }

        public Action<object> ParamaterizedAction { get; set; }

        public bool _CanExecute { get; set; }

        public bool IsParamaterized { get; set; }


        public Command(Action action, bool canExecute = true)
        {
            Action = action;
            _CanExecute = canExecute;
        }

        public Command(Action<object> paramaterizedAction, bool canExecute = true)
        {
            ParamaterizedAction = paramaterizedAction;
            _CanExecute = canExecute;
            IsParamaterized = true;
        }

        public bool CanExecute(object parameter)
            => _CanExecute;

        public void Execute(object parameter)
        {
            if (!IsParamaterized)
            {
                Action.Invoke();
            }
            else
            {
                ParamaterizedAction.Invoke(parameter);
            }
        }
    }
}