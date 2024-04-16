using System;
using UnityEngine;

namespace Builder
{
    public class InConstruction
    {
        private readonly float _startTime;
        private readonly float _buildTime;

        public event Action<InConstruction, bool> end;

        private bool _paused = false;

        public InConstruction(float buildTime)
        {
            _startTime = Time.time;
            _buildTime = buildTime;
        }

        public void Pause() => _paused = true;
        public void UnPause() => _paused = false;

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