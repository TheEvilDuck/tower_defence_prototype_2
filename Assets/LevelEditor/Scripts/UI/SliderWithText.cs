using System;
using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderWithText : MonoBehaviour,IObservableValue<int>
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_InputField _inputField;
    public event Action<int> changed;
    private int _defaultValue;

    public int Value => (int)_slider.value;

    public void RestoreDefaultValue()
    {
        _slider.value = _defaultValue;
        OnSliderValueChanged(_defaultValue);
    }

    public void ChangeBorders(int minValue, int maxValue)
    {
        _slider.minValue = minValue;
        _slider.maxValue = maxValue;

        _defaultValue = Mathf.Max(_defaultValue, minValue);
        _defaultValue = Mathf.Min(_defaultValue,maxValue);
        OnSliderValueChanged(_defaultValue);
    }

    private void Awake() 
    {
        _defaultValue = Value;
        OnSliderValueChanged(_defaultValue);
    }

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
    private void OnSliderValueChanged(float value)
    {
        _inputField.text = value.ToString();
        changed?.Invoke(Value);
    }
    private void OnInputValueChanged(string value)
    {
        int numberValue = Convert.ToInt32(value);
        numberValue = Mathf.Clamp(numberValue,(int)_slider.minValue,(int)_slider.maxValue);
        _inputField.text = numberValue.ToString();
        _slider.value = Convert.ToInt32(numberValue);
        changed?.Invoke(Value);
    }
}
