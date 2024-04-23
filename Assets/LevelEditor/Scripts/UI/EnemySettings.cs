using System;
using Enemies;
using LevelEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using Waves;

public class EnemySettings : MonoBehaviour
{
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _changeEnemyButton;
    [SerializeField] private RawImage _buttonImage;
    [SerializeField] private SliderWithText _slider;

    public event Action<EnemySettings> deleteButtonPressed;

    private EnemiesSelector _enemiesSelector;
    private GameObjectIconProvider<EnemyEnum> _gameObjectIconProvider;

    public EnemyEnum EnemyId {get; private set;}
    public int Count {get; private set;} = 1;

    public void Init(int maxEnemiesCount, EnemiesSelector enemiesSelector, GameObjectIconProvider<EnemyEnum> gameObjectIconProvider)
    {
        _enemiesSelector = enemiesSelector;
        _gameObjectIconProvider = gameObjectIconProvider;
        _buttonImage.texture = _gameObjectIconProvider.Get(EnemyId);
        _slider.ChangeBorders(1, maxEnemiesCount);

    }

    public void FillFromWaveEnemyData(WaveEnemyData waveEnemyData)
    {
        _slider.SetValue(waveEnemyData.count);
        EnemyId = waveEnemyData.enemyData.id;
        _buttonImage.texture = _gameObjectIconProvider.Get(EnemyId);
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
        _enemiesSelector.ShowWithPreloadedId(EnemyId);
        _enemiesSelector.gameObject.SetActive(true);

        _enemiesSelector.enemySelected += OnEnemySelected;
        _enemiesSelector.canceled += OnEnemiesSelectorCanceled;
    }

    private void OnEnemySelected(EnemyEnum enemyType)
    {
        OnEnemiesSelectorCanceled();
        EnemyId = enemyType;
        _buttonImage.texture = _gameObjectIconProvider.Get(EnemyId);
    }

    private void OnEnemiesSelectorCanceled()
    {
        _enemiesSelector.enemySelected -= OnEnemySelected;
        _enemiesSelector.canceled -= OnEnemiesSelectorCanceled;
    }

    private void OnSliderValueChanged(int value)
    {
        Count = value;
    }
}
