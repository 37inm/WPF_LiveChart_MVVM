using System;
using System.Windows.Input;

namespace WPF_LiveChart_MVVM.ViewModel.Command
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}
