using System.Collections.Generic;
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

    private Stack<Save> _saves;

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
        else if (input == "Back")
        {
            LoadSave();
        }
        else if (input == "=")
        {
            string str = PrintOutput().ToString();
            Clear();
            _numsStr[0] = _currentNumber = str;
        }
        else
        {
            if (_numsStr[0] != null)
            {
                if (_numsI < 2)
                    _numsI++;

                if (CheckingPriorities() == false && (input == "+" || input == "-") && _numsStr[2] == null)
                    LoadSave();
                
                _currentNumber = null;

                if (_signs[0] != null && _signs[1] != null && _numsStr[2] != null)
                {
                    if (CheckingPriorities() == false)
                    {
                        _numsStr[1] = Calculate(_signs[1], _numsStr[1], _numsStr[2]).ToString();
                        _numsStr[2] = null;

                        _signs[1] = null;
                    }
                    else
                    {
                        _numsStr[0] = Calculate(_signs[0], _numsStr[0], _numsStr[1]).ToString();
                        _numsStr[1] = _numsStr[2];
                        _numsStr[2] = null;

                        _signs[0] = _signs[1];
                        _signs[1] = null;
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

        if (input != "Back")
            Save();
    } 

    private double PrintOutput()
    {
        double num = 0;
        if (CheckingPriorities() == false)
        {
            if (_numsStr[2] == null)
                return 0;

            num = Calculate(_signs[1], _numsStr[1], _numsStr[2]);
            num = Calculate(_signs[0], _numsStr[0], num.ToString());
        }
        else
        {
            num = Calculate(_signs[0], _numsStr[0], _numsStr[1]);
            num = Calculate(_signs[1], num.ToString(), _numsStr[2]);
        }

        _output.text = num.ToString();

        return num;
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

    private void LoadSave()
    {
        if (_saves != null || _saves.Count > 0)
        {
            _saves.Pop();
            Save save = _saves.Peek();

            _currentNumber = save.CurrentNumber;
            _numsI = save.NumsI;

            for (int i = 0; i < _numsStr.Length; i++)
            {
                _numsStr[i] = save.NumsStr[i];
            }

            for (int i = 0; i < _signs.Length; i++)
            {
                _signs[i] = save.Signs[i];
            }
        }
    }

    private void Save()
    {
        _saves.Push(new Save(_currentNumber, _numsStr, _signs, _numsI));
    }

    private void Clear() 
    {
        _output.text = "0";
        _outputTwo.text = "";

        _currentNumber = null;

        _numsI = 0;
        _numsStr = new string[3];
        _signs = new string[2];

        _saves = new Stack<Save>();
        Save();
    }
}

public class Save
{
    public string CurrentNumber => _currentNumber;
    public string[] NumsStr => _numsStr;
    public string[] Signs => _signs;
    public int NumsI => _numsI;

    private string _currentNumber;

    private string[] _numsStr = new string[3];
    private string[] _signs = new string[2];

    private int _numsI;

    public Save(string currentNumber, string[] numsStr, string[] signs, int numsI)
    {
        _currentNumber = currentNumber;
        _numsI = numsI;

        for (int i = 0; i < numsStr.Length; i++)
        {
            _numsStr[i] = numsStr[i];
        }

        for (int i = 0; i < signs.Length; i++)
        {
            _signs[i] = signs[i];
        }
    }
}