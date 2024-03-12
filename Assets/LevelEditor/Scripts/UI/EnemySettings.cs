using System;
using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

public class EnemySettings : MonoBehaviour
{
    [SerializeField] private Button _deleteButton;
    [SerializeField] private TMP_Dropdown _dropDown;
    [SerializeField] private SliderWithText _slider;

    public event Action<EnemySettings> deleteButtonPressed;
    public event Action<EnemyEnum> typeChanged;
    public event Action<int> countChanged;

    private List<EnemyEnum>_enemyIds;

    public EnemyEnum EnemyId {get; private set;}
    public int Count {get; private set;} = 1;

    public void Init(EnemiesDatabase _enemiesDatabase, int maxEnemiesCount)
    {
        _dropDown.options.Clear();

        List<OptionData>_options = new List<OptionData>();
        _enemyIds = new List<EnemyEnum>();

        foreach (var Item in _enemiesDatabase.Items)
        {
            OptionData option = new OptionData(Item.Value.Name);
            _options.Add(option);
            _enemyIds.Add(Item.Key);
        }

        _dropDown.AddOptions(_options);

        _slider.ChangeBorders(1, maxEnemiesCount);
    }

    private void OnEnable() 
    {
        _deleteButton.onClick.AddListener(OnDeleteButtonPressed);
        _dropDown.onValueChanged.AddListener(OnDropDownValueChanged);
        _slider.changed+=OnSliderValueChanged;
    }

    private void OnDisable() 
    {
        _deleteButton.onClick.RemoveListener(OnDeleteButtonPressed);
        _dropDown.onValueChanged.RemoveListener(OnDropDownValueChanged);
        _slider.changed-=OnSliderValueChanged;
    }

    public void Delete() => OnDeleteButtonPressed();
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    private void OnDeleteButtonPressed()
    {
        deleteButtonPressed?.Invoke(this);
        Destroy(gameObject);
    }
    private void OnDropDownValueChanged(int optionId)
    {
        if (_enemyIds==null)
            throw new Exception("You forgot to init enemysettings of wave editor");

        if (optionId<0||optionId>=_enemyIds.Count)
            throw new ArgumentOutOfRangeException("Invalid option id of enemies option in wave editor");

        typeChanged?.Invoke(_enemyIds[optionId]);
        EnemyId = _enemyIds[optionId];
    }
    private void OnSliderValueChanged(int value)
    {
        Count = value;
    }
}
