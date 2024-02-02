using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderWithText : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_InputField _inputField;

    public int Value => (int)_slider.value;

    private void OnEnable() 
    {
        OnSliderValueChanged(_slider.value);
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
        _inputField.onEndEdit.AddListener(OnInputValueChanged);
    }
    
    private void OnDisable() 
    {
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        _inputField.onEndEdit.RemoveListener(OnInputValueChanged);
    }

    private void OnSliderValueChanged(float value) => _inputField.text = value.ToString();
    private void OnInputValueChanged(string value)
    {
        int numberValue = Convert.ToInt32(value);
        numberValue = Mathf.Clamp(numberValue,(int)_slider.minValue,(int)_slider.maxValue);
        _inputField.text = numberValue.ToString();
        _slider.value = Convert.ToInt32(numberValue);
    }
}
