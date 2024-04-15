using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Towers
{
    public abstract class Placable: MonoBehaviour
    {
        public event Action<Placable> destroyed;
        public bool CanBeDestroyed {get; private set;}
        public Vector3 Position => transform.position;
        public virtual void Init(bool canBeDestroyed)
        {
            CanBeDestroyed = canBeDestroyed;
        }

        public void DestroyPlacable()
        {
            if (!CanBeDestroyed)
                return;

            destroyed?.Invoke(this);

            Destroy(gameObject);
        }
        public virtual void OnBuild() {}
    }
}
