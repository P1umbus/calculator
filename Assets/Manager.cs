using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private Text _output;
    [SerializeField] private Text _output2;

    private string _sign; //Хранит в себе математический знак

    private string _numberStr; //Хранит в себе число в формате строки. Реализует возможность работы калькулятора с многозначными и отрицательными числами

    private double _newNumber; //Поле куда записываеться новые числа с ввода. Используеться для подсчетов.
    private double _res; //Хранит в себе резултат математических вычислений.

    private string _secondSign; //Тоже хранит в себе знак. Используеться в ситуации где разные приоритеты выполнения подсчета.
                                //Пример 2 + 2 * 2, в данной ситауции эта переменная будет хранить знак "*".
    private double _numberX; //Дополнительная переменная которая используеться в ситуации где разные приоритеты подсчета.

    private void Start()
    {
        Clear();
    }

    public void OnButtonClick()
    {
        string input = EventSystem.current.currentSelectedGameObject.name;

        if (input == "Clear")
        {
            Clear();
            return;
        }

        _output2.text += input; //!

        if (float.TryParse(input, out float num)) 
        {
            EnteringNum(input); //При вводе числа записываю его в строку, для возможности получения многозначного числа.
        }
        else
        {
            if (input == ",") //Логика кнопки ",".
            {
                EnteringСomma();
            }
            else if (input == "-" && _numberStr == null) //Реализация отрицательных чисел).
            {
                _numberStr = "-";
            }
            else
            {
                if (_numberStr == "-")
                    _numberStr = "-0";

                if (_secondSign != null && _numberStr == null) 
                {
                    _secondSign = null;
                }
                else if (_secondSign != null)
                {
                    PriorityCounting(input);
                    return;
                }

                if (_numberStr != null)
                    _newNumber = double.Parse(_numberStr);

                _numberStr = null;

                if (CheckingPriorities(input) == false)
                {
                    _secondSign = input;
                    return;
                }

                _res = Calculate(_sign, _res, _newNumber);

                if (input != "sqrt")
                    _sign = input;

                return;
            }
        }

        if (_secondSign != null)
        {
            _newNumber = Calculate(_secondSign, _newNumber, _numberX);
        }

        _output.text = Convert.ToString(Calculate(_sign, _res, _newNumber));
    }   

    private bool CheckingPriorities (string input) //Проверяет приоритет расчетов
    {
        bool fine = true;
        if ((_sign == "+" || _sign == "-") && (input == "*" || input == "/"))
            fine = false;

        return fine;
    }

    private void EnteringNum(string input) //Записывает число в виде строки в переменные
    {
        if (float.TryParse(input, out float num))
        {
            if (_numberStr != null)
                _numberStr += input;
            else
                _numberStr = input;

            if (_secondSign == null)
                _newNumber = double.Parse(_numberStr);
            else
                _numberX = double.Parse(_numberStr);
        }
        else
        {
            Debug.LogError("Этот метод может принимать только числа в виде строк!");
        }
    }

    private void EnteringСomma() //Реализация запятой
    {
        if (_numberStr != null && _numberStr.Length < 2)
            _numberStr += ',';
        else if (_numberStr == null)
            _numberStr = "0,";
    }

    private void PriorityCounting(string input) //Выполняет приоритетный подсчет.
    {
        _numberStr = null;

        _numberX = 0;

        if (CheckingPriorities(input) == false)
        {
            _secondSign = input;
        }
        else
        {
            _secondSign = null;

            _res = Calculate(_sign, _res, _newNumber);

            _sign = input;
        }
    }

    private double Calculate(string sign, double num1 = 0, double num2 = 0) //Выполняет расчет.
    {
        double res = 0;

        if (sign == null)
            sign = "+";

        switch (sign)
        {
            case "+":
                res = num1 + num2;
                break;
            case "-":
                res = num1 - num2;
                break;
            case "*":
                res = num1 * num2;
                break;
            case "/":
                res = num1 / num2;
                break;
        }

        return res;
    }

    private void Clear() // Сбрасывает все поля.
    {
        _sign = null;
        _secondSign = null;

        _output.text = "0";
        _output2.text = "";

        _newNumber = 0;
        _res = 0;

        _numberX = 0;

        _numberStr = null;
    }
}
