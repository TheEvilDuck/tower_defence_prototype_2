using System;
using Common.Interfaces;
using Towers;
using UnityEngine;

namespace Builder
{
    public class InConstruction: IPausable
    {
        private float _startTime;
        private readonly float _buildTime;

        public event Action<InConstruction, bool> end;

        private bool _paused = false;
        private float _pausedTime;
        public InConstruction(float buildTime)
        {
            _startTime = Time.time;
            _buildTime = buildTime;
        }

        public void Pause()
        {
            _paused = true;
            _pausedTime = Time.time;
        }
        public void UnPause()
        {
            _paused = false;
            float deltaPausedTime = Time.time - _pausedTime;
            _startTime += deltaPausedTime;
        }

        public void Cancel()
        {
            _paused = true;
            end?.Invoke(this, false);
        }

        public void Update()
        {
            if (_paused)
                return;

            if (Time.time - _startTime >= _buildTime)
            {
                _paused = true;
                end?.Invoke(this, true);
            }
        }
    }
}