using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCalculator.Models;

namespace WpfCalculator.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;  //Уведомляет об изменении полей

        // метод, уведомляющий об изменении поля после выполнения события
        void OnPropertyChanged([CallerMemberName] string PropertyName = null)  
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        private readonly Сalculation calculation;

        // свойство поля ввода
        private string input;
        public string Input
        {
            get => calculation.Input;
            set
            {
                input = value;
                OnPropertyChanged();
            }
        }

        // свойство поля вывода
        private string result;
        public string Result
        {
            get => calculation.Result;
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }

        public ICommand InsertCommand => new RelayCommand(o => Insert((string)o));
        public void Insert(string digit)
        {
            calculation.Insert(digit);
            OnPropertyChanged(nameof(Input));
            OnPropertyChanged(nameof(Result));
        }

        public ICommand InsertOperationCommand => new RelayCommand(o => InsertOperation((Operations)o));
        public void InsertOperation(Operations operation)
        {
            calculation.InsertOperation(operation);
            OnPropertyChanged(nameof(Input));
            OnPropertyChanged(nameof(Result));
        }
        public ICommand AddCommand { get; } //свойство
        private void OnAddCommandExecute(object p) //метод срабатывает при вызове команды
        {
        }
        private bool CanAddCommandExecuted(object p) //метод проверяет доступность команды и возвращает логический результат
        {
            return true;
        }

        // конструктор для инициализации команды AddCommand через класс RelayCommand
        public MainWindowViewModel()
        {
            AddCommand = new RelayCommand(OnAddCommandExecute, CanAddCommandExecuted);
            calculation = new Сalculation();
        }
    }
}
