using System;
using System.Windows.Input;

namespace Isi.Utility.ViewModels
{
    public class DelegateCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly Action<T> executeAction;
        private readonly Func<T, bool> canExecuteFunction;

        public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecuteFunction = null)
        {
            this.executeAction = executeAction;
            this.canExecuteFunction = canExecuteFunction;
        }

        public bool CanExecute(object parameter)
        {
            T typedParameter = (T)parameter;
            return canExecuteFunction?.Invoke(typedParameter) ?? true;
        }

        public void Execute(object parameter)
        {
            T typedParameter = (T)parameter;
            if (canExecuteFunction?.Invoke(typedParameter) ?? true)
                executeAction?.Invoke(typedParameter);
        }

        public void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(sender: this, new EventArgs());
        }
    }
}
