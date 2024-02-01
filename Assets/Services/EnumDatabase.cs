using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics;
using UnityEngine;

namespace Services
{
    public abstract class EnumDataBase<TEnum,TConfig> : ScriptableObject where TEnum: Enum where TConfig: ScriptableObject
    {
        [Serializable]
        public class SerializableDictionaryItem<TKey,TValue>
        {
            [field: SerializeField]public TKey Key;
            [field: SerializeField]public TValue Value;
        }

        [SerializeField] private SerializableDictionaryItem<TEnum,TConfig>[] _content;

        public IEnumerable<SerializableDictionaryItem<TEnum,TConfig>> Items => _content;

        public bool ContainsKey(TEnum id)
        {
            foreach (var item in _content)
            {
                if (EqualityComparer<TEnum>.Default.Equals(id , item.Key))
                    return true;
            }

            return false;
        }

        public bool TryGetValue(TEnum id, out TConfig config)
        {
            config = null;

            if (!ContainsKey(id))
                return false;

            config = _content.First((item) => EqualityComparer<TEnum>.Default.Equals(id , item.Key)).Value;

            return true;
        }
    }
}
