using TMPro;
using UnityEngine;

namespace GamePlay.UI
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;

        public void UpdateText(int money)
        {
            _moneyText.text = money.ToString();
        }
    }
}
