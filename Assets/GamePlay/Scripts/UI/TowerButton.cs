using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GamePlay.UI
{
    public class TowerButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private RawImage _icon;

        public UnityEvent onClick => _button.onClick;

        public void UpdateInfo(string text, Texture2D texture)
        {
            _text.text = text;
            _icon.texture = texture;
        }
    }
}
