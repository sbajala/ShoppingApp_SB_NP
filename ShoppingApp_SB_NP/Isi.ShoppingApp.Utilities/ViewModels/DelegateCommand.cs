using System;
using System.Windows.Input;

namespace Isi.Utility.ViewModels
{
    public class DelegateCommand : DelegateCommand<object>, ICommand
    {


        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteFunction = null)
            : base(executeAction, canExecuteFunction)
        { }
    }
}
