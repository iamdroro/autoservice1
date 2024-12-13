﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Autocervice.Commands
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) => !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);

        public async void Execute(object parameter)
        {
            _isExecuting = true;
            try
            {
                await _execute(parameter);
            }
            finally
            {
                _isExecuting = false;
            }
        }
    }
}

