using EyeClinic.Model;
using System;
using System.Windows.Input;

namespace EyeClinic.WPF.Base
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Predicate<bool> _canExecute;
        private ICommand getByName;

        private event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Action execute)
            : this(execute, DefaultCanExecute) {
        }
        public RelayCommand(Action execute, Predicate<bool> canExecute) {
            if (canExecute != null) {
                _execute = execute ?? throw new ArgumentNullException(nameof(execute));
                _canExecute = canExecute;
            } else {
                throw new ArgumentNullException(nameof(canExecute));
            }
        }

        public RelayCommand()
        {
        }

        public RelayCommand(ICommand getByName)
        {
            this.getByName = getByName;
        }

        public event EventHandler CanExecuteChanged {
            add {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }

            remove {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            parameter ??= true;

            return _canExecute != null && _canExecute((bool)parameter);
        }

        public void Execute(object parameter) {
            _execute();
        }

        public void OnCanExecuteChanged() {
            var handler = CanExecuteChangedInternal;
            handler?.Invoke(this, EventArgs.Empty);
        }

        private static bool DefaultCanExecute(bool parameter) {
            return true;
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private Action<T> _execute;
        private Predicate<T> _canExecute;
        private Action<CustomerDto> deleteDisease;

        private event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Action<T> execute)
            : this(execute, DefaultCanExecute) {
        }
        public RelayCommand(Action<T> execute, Predicate<T> canExecute) {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        public RelayCommand(Action<CustomerDto> deleteDisease)
        {
            this.deleteDisease = deleteDisease;
        }

        public event EventHandler CanExecuteChanged {
            add {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }

            remove {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter) {
            return _canExecute != null && _canExecute((T)parameter);
        }

        public void Execute(object parameter) {
            _execute((T)parameter);
        }

        public void OnCanExecuteChanged() {
            var handler = CanExecuteChangedInternal;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public void Destroy() {
            _canExecute = _ => false;
            _execute = _ => { };
        }

        private static bool DefaultCanExecute(T parameter) {
            return true;
        }
    }

}