using System;
using System.Windows.Input;

namespace EmoteTool.ViewModels
{
    internal class CommandFactory : ICommand
    {
        public Action Action { get; set; }

        public Action<object> ParamaterizedAction { get; set; }

        public bool _CanExecute { get; set; }

        public bool IsParamaterized { get; set; }

        public CommandFactory(Action action, bool canExecute = true)
        {
            Action = action;
            _CanExecute = canExecute;
        }

        public CommandFactory(Action<object> paramaterizedAction, bool canExecute = true)
        {
            ParamaterizedAction = paramaterizedAction;
            _CanExecute = canExecute;
            IsParamaterized = true;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _CanExecute;
        }

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