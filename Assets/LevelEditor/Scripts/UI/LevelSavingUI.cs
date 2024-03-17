using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class LevelSavingUI: MonoBehaviour
    {
        [SerializeField]Transform _rotatableCircle;
        [SerializeField]float _rotationSpeed = 1f;
        [SerializeField]TMP_Text _text;
        [SerializeField]Button _okButton;

        private void Update() 
        {
            if (!_rotatableCircle.gameObject.activeSelf)
                return;

            _rotatableCircle.Rotate(new Vector3(0,0,_rotationSpeed*Time.deltaTime));
        }

        private void OnEnable() 
        {
            _okButton.onClick.AddListener(Hide);
        }

        private void OnDisable() 
        {
            _okButton.onClick.RemoveListener(Hide);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _rotatableCircle.gameObject.SetActive(true);
            _okButton.gameObject.SetActive(false);
            _text.gameObject.SetActive(false);
        }

        public void OnLevelSaveTried(string message) => ShowMessage(message);

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ShowMessage(string message)
        {
            _text.text = message;
            _text.gameObject.SetActive(true);
            _rotatableCircle.gameObject.SetActive(false);
            _okButton.gameObject.SetActive(true);
        }
    }
}
