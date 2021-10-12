using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private Text _output;
    [SerializeField] private Text _output2;

    private string _sign; 

    private string _numberStr; 

    private double _newNumber; 
    private double _res; 

    private string _secondSign; 
                                
    private double _numberX; 

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

        //_output2.text += input; 

        if (float.TryParse(input, out float num)) 
        {
            EnteringNum(input); 
        }
        else
        {
            if (input == ",") 
            {
                EnteringComma();
            }
            else if (input == "-" && _numberStr == null) 
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

                if (_sign != null)
                    _output2.text = $"{_res} {_sign} {_newNumber}";
                else if (_secondSign != null)
                    _output2.text = $"{_res} {_sign} {_newNumber} {_secondSign} {_numberX}";

                return;
            }
        }

        if (_secondSign != null)
        {
            _newNumber = Calculate(_secondSign, _newNumber, _numberX);
        }

        _output.text = Convert.ToString(Calculate(_sign, _res, _newNumber));

        if (_secondSign != null)
            _output2.text = $"{_res} {_sign} {_newNumber}";
        else if (_sign != null)
            _output2.text = $"{_res} {_sign} {_newNumber} {_secondSign} {_numberX}";
        else
            _output2.text = _newNumber.ToString();
    }   

    private void Output2()
    {

    }

    private bool CheckingPriorities (string input) 
    {
        bool fine = true;
        if ((_sign == "+" || _sign == "-") && (input == "*" || input == "/"))
            fine = false;

        return fine;
    }

    private void EnteringNum(string input) 
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

    private void EnteringComma() 
    {
        if (_numberStr != null)
            _numberStr += ',';
        else if (_numberStr == null)
            _numberStr = "0,";
    }

    private void PriorityCounting(string input) 
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

    private double Calculate(string sign, double num1 = 0, double num2 = 0) 
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

    private void Clear() 
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
