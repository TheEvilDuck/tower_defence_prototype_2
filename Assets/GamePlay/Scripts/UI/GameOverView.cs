using System;
using Common.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI
{
    public class GameOverView : MonoBehaviour, IMenuParent
    {
        private const string WIN_TEXT = "YOU WIN!";
        private const string LOSE_TEXT = "YOU LOST!";

        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TextMeshProUGUI _gameOverText;

        public bool Active => gameObject.activeInHierarchy;

        public event Action restartButtonPressed;
        public event Action exitButtonPressed;

        private void OnEnable() 
        {
            _restartButton.onClick.AddListener(OnRestartButtonPressed);
            _exitButton.onClick.AddListener(OnExitButtonPressed);
        }

        private void OnDisable() 
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonPressed);
            _exitButton.onClick.RemoveListener(OnExitButtonPressed);
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        public void ShowLoseText()
        {
            Show();
            _gameOverText.text = LOSE_TEXT;
        }

        public void ShowWinText()
        {
            Show();
            _gameOverText.text = WIN_TEXT;
        }

        private void OnRestartButtonPressed() => restartButtonPressed?.Invoke();
        private void OnExitButtonPressed() => exitButtonPressed?.Invoke();

        
    }

}