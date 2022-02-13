
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfCalculator.Models
{
    public interface IOperations
    {
        void Insert(string digit);
        void InsertOperation(Operations operation);
    }

    public enum Operations
    {
        BracketOpen,
        BracketClouse,
        Clear,
        Backspace,
        Percent,
        Pow2,
        Sqrt,
        Negative,
        Equal
    }

    public class Сalculation : IOperations
    {
        public string Input { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;

        private const string OpenBracketSign = "(";
        private const string EncloseBracketSign = ")";

        private int openBracketCont = 0;
        private int closeBracketCont = 0;
        private readonly Stack<string> encloseBracketStack;

        public Сalculation()
        {
            encloseBracketStack = new Stack<string>();
        }

        //ввод данных
        public void Insert(string element)
        {
            if ((element != "/") ||
                (element != "*") ||
                (element != "+") ||
                (element != "-") ||
                (element != "%") ||
                (element != "x") ||
                (element != "√") ||
                (element != "(") ||
                (element != ")"))
            {
                Input += element;
                Result = CalculateExpression();
            }
            Result = CalculateExpression();
        }

        private void BracketOpen()  // открытие скобок
        {
            Input += OpenBracketSign;
            encloseBracketStack.Push(EncloseBracketSign);
            openBracketCont++;
            Result = string.Empty;
        }
        private void BracketClose()  // закрытие скобок
        {
            Input += EncloseBracketSign;
            if (encloseBracketStack.Count > 0)
            {
                encloseBracketStack.Pop();
                closeBracketCont++;
                openBracketCont--;
                Result = CalculateExpression();
            }
            else
                Result = "некорректно расставлены скобки";
        }

        private void Clear()  //Очистка поля ввода
        {
            Input = string.Empty;
            Result = string.Empty;
        }

        private void Percent()  // вычисление процента
        {
            Input = Result;
            Input = Convert.ToString(Convert.ToDouble(Result) / 100);
            Result = Input;
        }

        private void Pow2()  // вычисление квадрата числа
        {
            if (Result == null || Result == "")
            {
                Input = Convert.ToString(Math.Pow(Convert.ToDouble(Input), 2));
                Result = Input;
            }
            Input = Convert.ToString(Math.Pow(Convert.ToDouble(Result), 2));
            Result = Input;
        }

        private void Sqrt()  // вычисление квадратног корня
        {
            if (Result == null || Result == "")
            {
                Result = Convert.ToString(Math.Sqrt(Convert.ToDouble(Input)));
                //Result = CalculateExpression();
            }
            Input = Convert.ToString(Math.Sqrt(Convert.ToDouble(Result)));
            Result = CalculateExpression();
        }

        private void Negative()  // установка +/- числа
        {
            if (Input == null || Input == "")
            {
                Input = "1";
                Result = "1";
            }
            if (Result == null || Result == "")
            {
                Input = Convert.ToString(Convert.ToDouble(Input)* -1);
                Result = CalculateExpression();
            }
            
            Input = Convert.ToString(Convert.ToDouble(Input) * -1);
            Result = CalculateExpression();
        }

        private void Backspace() // удаление последнего введенного значения
        {
            if (string.IsNullOrEmpty(Input)) return;

            //действия при скобках
            if (Regex.IsMatch(Input, $@"\{EncloseBracketSign}&"))
            {
                //проверка корректности скобок перед очисткой стека и удалении 
                if (closeBracketCont <= openBracketCont)
                {
                    encloseBracketStack.Push(EncloseBracketSign);
                    closeBracketCont--;
                }
                else
                    closeBracketCont--;
            }

            if (Regex.IsMatch(Input, $@"\{OpenBracketSign}"))
            {
                if (closeBracketCont <= openBracketCont)
                {

                    encloseBracketStack.Pop();
                    openBracketCont--;
                }
            }
            Input = Input.Remove(Input.Length - 1, 1);
            Result = CalculateExpression();
        }

        private void Equal() // вычисление значения
        {
            Input = Result;
        }

        //операции 
        public void InsertOperation(Operations operation)
        {
            switch (operation)
            {
                case Operations.BracketOpen:
                    BracketOpen();
                    break;
                case Operations.BracketClouse:
                    BracketClose();
                    break;
                case Operations.Clear:
                    Clear();
                    break;
                case Operations.Backspace:
                    Backspace();
                    break;
                case Operations.Percent:
                    Percent();
                    break;
                case Operations.Pow2:
                    Pow2();
                    break;
                case Operations.Sqrt:
                    Sqrt();
                    break;
                case Operations.Negative:
                    Negative();
                    break;
                case Operations.Equal:
                    Equal();
                    break;
            }
        }

        // рассчет
        public string CalculateExpression()
        {
            if (string.IsNullOrEmpty(Input) || !Regex.IsMatch(Input.Last().ToString(), @"(\d|\))") || encloseBracketStack.Any())
                return string.Empty;

            Expression expression = new Expression(Input);
            return expression.calculate().ToString(CultureInfo.InvariantCulture);
        }
    }
}
