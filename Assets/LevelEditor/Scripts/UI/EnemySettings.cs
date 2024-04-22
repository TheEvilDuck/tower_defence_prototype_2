using System;
using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using Enemies;
using LevelEditor.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Waves;
using static TMPro.TMP_Dropdown;

public class EnemySettings : MonoBehaviour
{
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _changeEnemyButton;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private SliderWithText _slider;

    public event Action<EnemySettings> deleteButtonPressed;
    public event Action<EnemyEnum> typeChanged;
    public event Action<int> countChanged;

    private EnemiesSelector _enemiesSelector;

    public EnemyEnum EnemyId {get; private set;}
    public int Count {get; private set;} = 1;

    public void Init(int maxEnemiesCount, EnemiesSelector enemiesSelector)
    {
        _enemiesSelector = enemiesSelector;
        _slider.ChangeBorders(1, maxEnemiesCount);

    }

    public void FillFromWaveEnemyData(WaveEnemyData waveEnemyData)
    {
        _slider.SetValue(waveEnemyData.count);
    }

    private void OnEnable() 
    {
        _deleteButton.onClick.AddListener(OnDeleteButtonPressed);
        _slider.changed+=OnSliderValueChanged;
        _changeEnemyButton.onClick.AddListener(OnEnemyTypeButtonPressed);
    }

    private void OnDisable() 
    {
        _deleteButton.onClick.RemoveListener(OnDeleteButtonPressed);
        _slider.changed-=OnSliderValueChanged;
        _changeEnemyButton.onClick.RemoveListener(OnEnemyTypeButtonPressed);
    }

    public void Delete() => OnDeleteButtonPressed();
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    private void OnDeleteButtonPressed()
    {
        deleteButtonPressed?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnEnemyTypeButtonPressed()
    {
        _enemiesSelector.gameObject.SetActive(true);

        _enemiesSelector.enemySelected += OnEnemySelected;
    }

    private void OnEnemySelected(EnemyEnum enemyType, string enemyName)
    {
        _enemiesSelector.enemySelected -= OnEnemySelected;

        _buttonText.text = enemyName;

        typeChanged?.Invoke(enemyType);
    }

    private void OnSliderValueChanged(int value)
    {
        Count = value;
        countChanged?.Invoke(Count);
    }
}
