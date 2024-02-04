using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Towers
{
    public abstract class Placable: MonoBehaviour
    {
        public event Action destroyed;
        public bool CanBeDestroyed {get; private set;}
        public virtual void Init(bool canBeDestroyed)
        {
            CanBeDestroyed = canBeDestroyed;
        }

        public void DestroyPlacable()
        {
            if (!CanBeDestroyed)
                return;

            destroyed?.Invoke();

            Destroy(gameObject);
        }
        public virtual void OnBuild() {}
    }
}
