using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Autocervice.Commands;

namespace Autocervice.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Метод для создания команд без параметров
        public ICommand CreateCommand(Action execute, Func<bool> canExecute = null)
        {
            return new RelayCommand(param => execute(), param => canExecute?.Invoke() ?? true);
        }

        // Метод для создания команд с параметрами
        public ICommand CreateCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            return new RelayCommand(execute, canExecute);
        }


        // Метод для создания асинхронных команд
        public ICommand CreateAsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            return new AsyncRelayCommand(execute, canExecute);
        }
    }
}


