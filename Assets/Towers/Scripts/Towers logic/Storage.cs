using System.Collections;
using System.Collections.Generic;
using GamePlay.Player;
using Towers.Configs;
using UnityEngine;

namespace Towers
{
    public class Storage : Placable
    {
        private PlayerStats _playerStats;
        private int _moneyChange;
        public void Init(StorageConfig storageConfig, PlayerStats playerStats)
        {
            _playerStats = playerStats;
            _moneyChange = storageConfig.Money;
        }
        public override void Pause()
        {
            
        }

        public override void UnPause()
        {
            
        }

        public override void OnBuild()
        {
            _playerStats.IncreaseMaxMoney(_moneyChange);
        }

        public override void OnDestroyed()
        {
            _playerStats.DecreaseMaxMoney(_moneyChange);
        }
    }

}