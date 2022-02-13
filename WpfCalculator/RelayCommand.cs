using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfCalculator
{
    // Класс для реализации наследования от него команд

    class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        //событие срабатывает при изменении состояния CanExecute
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // конструктор, связывающий делегаты Action и Func с методами Execute и CanExecute
        public RelayCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            canExecute = CanExecute;
        }

        //метод проверяет доступность команды и возвращает логический результат
        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        //метод срабатывает при вызове команды
        public void Execute(object parameter) => execute(parameter);
    }
}
