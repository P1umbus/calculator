using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private Text _output;
    [SerializeField] private Text _outputTwo;

    private string _currentNumber; 

    private string[] _numsStr = new string[3];
    private string[] _signs = new string[2];

    private int _numsI;

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

        if (float.TryParse(input, out float num) || input == ",")
        {
            EnteringNum(input);
        }
        else
        {
            if (_numsStr[0] == null)
            {
                return;
            }
            else
            {
                _currentNumber = null;

                if (_numsI < 2)
                    _numsI++;

                if (_signs[0] != null && _signs[1] != null)
                {
                    if (CheckingPriorities() == false)
                    {
                        _numsStr[1] = Calculate(_signs[1], _numsStr[1], _numsStr[2]).ToString();
                        _numsStr[2] = null;
                    }
                    else
                    {
                        _numsStr[0] = Calculate(_signs[0], _numsStr[0], _numsStr[1]).ToString();
                        _numsStr[1] = _numsStr[2];
                        _numsStr[2] = null;
                    }
                }

                if (_signs[0] == null)
                    _signs[0] = input;
                else if (_numsStr[1] == null)
                    _signs[0] = input;
                else
                    _signs[1] = input;
            }
        }

        PrintOutput();
        PrintOutputTwo();
    } 

    private void PrintOutput()
    {
        if (CheckingPriorities() == false)
        {
            if (_numsStr[2] == null)
                return;

            double num = Calculate(_signs[1], _numsStr[1], _numsStr[2]);
            _output.text = Calculate(_signs[0], _numsStr[0], num.ToString()).ToString();
        }
        else
        {
            double num = Calculate(_signs[0], _numsStr[0], _numsStr[1]);
            _output.text = Calculate(_signs[1], num.ToString(), _numsStr[2]).ToString();
        }
    }

    private void PrintOutputTwo()
    {
        _outputTwo.text = $"{_numsStr[0] ?? ""}{_signs[0] ?? ""}{_numsStr[1] ?? ""}{_signs[1] ?? ""}{_numsStr[2] ?? ""}";
    }

    private bool CheckingPriorities()
    {
        bool fine = true;
        if ((_signs[0] == "+" || _signs[0] == "-") && (_signs[1] == "*" || _signs[1] == "/"))
            fine = false;

        return fine;
    }

    private void EnteringNum(string input)
    {
        if (input == ",")
        {
            EnteringComma();
        }
        else if (_currentNumber != null)
        {
            if (_currentNumber == "0" && input == "0")
                return;
            else if (_currentNumber.Length > 10)
                return;

            _currentNumber += input;
        }
        else
        {
            _currentNumber = input;
        }

        _numsStr[_numsI] = _currentNumber;
    }

    private void EnteringComma() 
    {
        if (_currentNumber == null)
            _currentNumber = "0,";
        else if (_currentNumber.Contains(","))
            Debug.Log("Тут уже есть запятая");
        else
            _currentNumber += ',';        
    }

    private double Calculate(string sign, string numStr1, string numStr2) 
    {
        double res = 0;

        numStr1 = Check(numStr1);
        numStr2 = Check(numStr2);

        double num1 = double.Parse(numStr1 ?? "0");
        double num2 = double.Parse(numStr2 ?? "0");        

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

    private string Check(string str)
    {
        if (str != null)
        {
            if (str.EndsWith(","))
                str = str.Remove(str.Length - 1);
        }
        return str;
    }

    private void Clear() 
    {
        _output.text = "0";
        _outputTwo.text = "";

        _currentNumber = null;

        _numsI = 0;
        _numsStr = new string[3];
        _signs = new string[2];
    }
}
