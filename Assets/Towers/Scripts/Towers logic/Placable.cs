using System;
using Common.Interfaces;
using UnityEngine;

namespace Towers
{
    public abstract class Placable: MonoBehaviour, IPausable
    {
        public event Action<Placable> destroyed;
        public bool CanBeDestroyed {get; protected set;}
        public Vector3 Position => transform.position;
        public virtual void Init(bool canBeDestroyed)
        {
            CanBeDestroyed = canBeDestroyed;
        }

        public bool DestroyPlacable()
        {
            if (!CanBeDestroyed)
                return false;

            OnDestroyed();
            destroyed?.Invoke(this);

            Destroy(gameObject);

            return true;
        }

        public void ForceDestroy()
        {
            destroyed?.Invoke(this);

            Destroy(gameObject);
        }
        public virtual void OnBuild() {}
        public virtual void OnDestroyed() {}

        public abstract void Pause();

        public abstract void UnPause();
    }
}
