using TMPro;
using UnityEngine;

namespace GamePlay.UI
{
    public class MainBuildingHealth : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;

        public void UpdateHealthText(int health)
        {
            _healthText.text = $"Health: {health}";
        }
    }
}
