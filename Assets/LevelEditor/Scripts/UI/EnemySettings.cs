using System;
using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using Enemies;
using UnityEngine;
using UnityEngine.UI;

public class EnemySettings : MonoBehaviour
{
    [SerializeField] private Button _deleteButton;

    public event Action<EnemySettings> deleteButtonPressed;
    public event Action<EnemyEnum> typeChanged;
    public event Action<int> countChanged;

    private void OnEnable() 
    {
        _deleteButton.onClick.AddListener(OnDeleteButtonPressed);
    }

    private void OnDisable() 
    {
        _deleteButton.onClick.RemoveListener(OnDeleteButtonPressed);
    }

    public void Delete() => OnDeleteButtonPressed();
    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    private void OnDeleteButtonPressed()
    {
        deleteButtonPressed?.Invoke(this);
        Destroy(gameObject);
    }
}
