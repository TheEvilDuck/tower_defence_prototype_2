using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Enemies database", menuName = "Enemies/New enemies database")]
    public class EnemiesDataBaseSO : ScriptableObject
    {
        [SerializeField]private List<IdAndPrefab>_enemies;


        private void OnEnable() {
            Debug.Log("A");
        }

        private void OnValidate() 
        {
            var AllIds = Enum.GetValues(typeof(EnemyId));

            if (_enemies.Count!=AllIds.Length)
            {
                foreach (var id in AllIds)
                {
                    IdAndPrefab idAndPrefab = new IdAndPrefab();
                    idAndPrefab.Id = (EnemyId)id;
                    _enemies.Add(idAndPrefab);
                }
            }


            
        }
    }

    [Serializable]
    internal class IdAndPrefab
    {
        [SerializeField]public EnemyId Id;
        [SerializeField]public GameObject Prefab;
    }
}
