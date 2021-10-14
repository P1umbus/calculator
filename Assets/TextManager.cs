using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(RectTransform))]

public class TextManager : MonoBehaviour
{
    [SerializeField] private RectTransform _contentRectTransform;
    [SerializeField] private float _hz;

    private Text _text;
    RectTransform _rectTransform;

    private void Start()
    {
        _text = GetComponent<Text>();

        _rectTransform = GetComponent<RectTransform>();

        _contentRectTransform.sizeDelta = _rectTransform.sizeDelta;
    }

    private void Update()
    {
        if (_text.text.Length * _hz > _rectTransform.sizeDelta.y)
        {
            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y + 120);
            _contentRectTransform.sizeDelta = _rectTransform.sizeDelta;
        }
    }
}
