using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private Text _output;
    [SerializeField] private Text _output2;

    private string _sign;

    private double[] _numbers = new double[2];
    private int _i;

    private string _numberStr;
    private string _secondSign;

    private double _numberX;

    public void OnButtonClick()
    {
        string input = EventSystem.current.currentSelectedGameObject.name;

        if (input == "Clear")
        {
            Clear();
            return;
        }

        _output2.text += input;

        if (float.TryParse(input, out float num))
        {
            if (_numberStr != null)
                _numberStr += input;
            else
                _numberStr = input;
        }
        else
        {
            if (input == ",")
            {
                if (_numberStr != null)
                    _numberStr += ',';
                else
                    _numberStr = "0,";
            }
            else if (input == "-" && _numberStr == null)
            {
                _numberStr = "-";
            }
            else
            {
                ParseNumStrToNumbers();

                if (input == "sqrt")
                {                    
                    Sqrt();
                    return;
                }

                if (_secondSign != null)
                    CUWLName();

                if ((_sign == "+" || _sign == "-") && (input == "*" || input == "/"))
                {
                    _secondSign = input;
                    return;
                }
                else if (_sign != null)
                {
                    Calculate();
                }

                _sign = input;
            }
        }
    }

    private void ParseNumStrToNumbers()
    {
        if (_secondSign != null)
        {
            _numberX = float.Parse(_numberStr);
        }
        else
        {
            if (_i > 1)
                _i = 0;
            _numbers[_i] = float.Parse(_numberStr);
            _i++;
        }

        _numberStr = null;
    }

    private void Calculate()
    {
        if (_sign == null)
            _sign = "+";

        switch (_sign)
        {
            case "+":
                _numbers[0] += _numbers[1];
                _i = 1;
                _output.text = Convert.ToString(_numbers[0]);
                break;
            case "-":
                _numbers[0] -= _numbers[1];
                _i = 1;
                _output.text = Convert.ToString(_numbers[0]);
                break;
            case "*":
                _numbers[0] *= _numbers[1];
                _i = 1;
                _output.text = Convert.ToString(_numbers[0]);
                break;
            case "/":
                _numbers[0] /= _numbers[1];
                _i = 1;
                _output.text = Convert.ToString(_numbers[0]);
                break;
            case "=":
                _output.text = Convert.ToString(_numbers[0]);
                _output2.text += _numbers[0];
                break;
        }
    }

    private void Clear()
    {
        _sign = null;

        _output.text = "0";
        _output2.text = "";

        _i = 0;

        _numbers = new double[2];
    }

    private void CUWLName()
    {
        if (_secondSign == "*")
            _numbers[1] *= _numberX;
        else if (_secondSign == "/")
            _numbers[1] /= _numberX;

        _secondSign = null;
    }

    private void Sqrt()
    {
        if (_i > 1)
            _i = 0;
        _i++;
        if (_i > 1)
            _i = 0;

        _numbers[_i] = Math.Sqrt(_numbers[_i]);

        _i++;

        Calculate();
    }
}
