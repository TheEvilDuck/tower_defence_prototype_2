using System;
using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using UnityEngine;

namespace Towers
{
    public class MainBuilding : Placable, IDamagable
    {
        private int _health;
        private int _maxHealth;
        private bool _paused;

        public void Init(int maxHealth)
        {
            _maxHealth = maxHealth;
            _health = _maxHealth;
        }

        public override void Pause()
        {
            _paused = true;
        }

        public override void UnPause()
        {
            _paused = false;
        }

        public void TakeDamage(int damage)
        {
            if (_health <= 0)
                return;

            _health -= damage;

            if (_health <= 0)
            {
                CanBeDestroyed = true;
                DestroyPlacable();
            }
        }
    }
}
