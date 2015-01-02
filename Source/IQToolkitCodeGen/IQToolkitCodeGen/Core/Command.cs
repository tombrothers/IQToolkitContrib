using System;
using System.Diagnostics.Contracts;
using System.Windows.Input;

namespace IQToolkitCodeGen.Core {
    public class Command : Command<object> {
        public Command(Action<object> execute, Predicate<object> canExecute = null)
            : base(execute, canExecute) {
        }
    }

    public class Command<T> : ICommand {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public Command(Action<T> execute, Predicate<T> canExecute = null)
        {
            ArgumentUtility.CheckNotNull("execute", execute);

            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            if (this._canExecute == null) {
                return true;
            }

            return this._canExecute((T)parameter);
        }

        public void Execute(object parameter) {
            this._execute((T)parameter);
        }
    }
}
