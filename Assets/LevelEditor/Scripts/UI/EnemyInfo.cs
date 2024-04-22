using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor.UI
{
    public class EnemyInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _enemyName;
        [SerializeField] private TextMeshProUGUI _stats;
        [SerializeField] private TextMeshProUGUI _desc;
        [SerializeField] private Transform _gameObjectViewPlace;

        private Enemy _enemy;

        private void Awake() 
        {
            _enemyName.text = string.Empty;
            _stats.text = "Click on enemy name to get info";
            _desc.text = string.Empty;
        }

        public void UpdateInfo(EnemyConfig enemyConfig)
        {
            _enemyName.text = enemyConfig.Name;
            
            if (_enemy != null)
                Destroy(_enemy.gameObject);

            _enemy = Instantiate(enemyConfig.Prefab, _gameObjectViewPlace);
            _enemy.transform.localPosition = Vector3.zero;

            _stats.text = $"Speed: {enemyConfig.WalkSpeed}\nDamage: {enemyConfig.Damage}\nHealth: {enemyConfig.MaxHealth}\nAttack rate: {enemyConfig.AttackRate}\nAttack range: {enemyConfig.Range}";

        }
    }
}
