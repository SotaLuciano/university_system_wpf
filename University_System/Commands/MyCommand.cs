using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace University_System.Commands
{
    public class MyCommand : ICommand
    {
        private readonly Action _executed;
        private readonly Func<bool> _canExecute;


        public MyCommand(Action executed)
        {
            _executed = executed;
        }
        public MyCommand(Action executed, Func<bool> canExecute)
        {
            _executed = executed;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            _executed();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
