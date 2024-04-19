using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class GameObjectPool<TObject> where TObject: MonoBehaviour
    {
        private List<TObject> _allobjects;
        private Queue<TObject> _objects;

        public GameObjectPool(int size, TObject prefab)
        {
            GameObject poolParent = new GameObject($"{prefab.name} pool");
            _allobjects = new List<TObject>();
            _objects = new Queue<TObject>(size);

            for (int i = 0; i < size; i++)
            {
                TObject instantiatedPrefab = UnityEngine.GameObject.Instantiate<TObject>(prefab, poolParent.transform);
                instantiatedPrefab.gameObject.SetActive(false);

                _objects.Enqueue(instantiatedPrefab);
                _allobjects.Add(instantiatedPrefab);
            }
        }

        public TObject Get()
        {
            TObject result = null;

            if (_objects.Count == 0)
                Debug.LogWarning($"You should increase {typeof(TObject)} pool capacity");

            result = _objects.Dequeue();

            return result;
        }

        public void Return(TObject returnedObject)
        {
            if (!_allobjects.Contains(returnedObject))
                throw new ArgumentException($"Returned to pool object doesn't belong to this pool, {typeof(TObject)}");

            returnedObject.gameObject.SetActive(false);
            _objects.Enqueue(returnedObject);
        }
    }
}
