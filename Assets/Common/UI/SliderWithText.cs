using System;
using Common.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class SliderWithText : MonoBehaviour,IObservableValue<int>
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_InputField _inputField;
        public event Action<int> changed;
        private int _defaultValue;

        public int Value => (int)_slider.value;

        public void ChangeBorders(int minValue, int maxValue)
        {
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;

            _defaultValue = Mathf.Max(_defaultValue, minValue);
            _defaultValue = Mathf.Min(_defaultValue,maxValue);
            OnSliderValueChanged(_defaultValue);
        }

        public void SetValue(int newValue)
        {
            if (newValue<_slider.minValue||newValue>_slider.maxValue)
                throw new ArgumentOutOfRangeException("Value is our of slider's range");

            _inputField.text = newValue.ToString();
            _slider.value = newValue;
            changed?.Invoke(Value);
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
}
