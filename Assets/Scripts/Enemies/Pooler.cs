using System;
using System.Collections.Generic;

using UnityEngine;

namespace AceInTheHole
{
    [Serializable]
    public class Pooler<T>
        where T : MonoBehaviour
    {
        public List<T> pool = new List<T>();
        public T prefab;
        private Func<T, T> spawnNewInstance;

        public void Initialise(Func<T, T> spawnNewInstance)
        {
            this.spawnNewInstance = spawnNewInstance;
        }

        public T Get()
        {
            int count = pool.Count;
            for (int i = 0; i < count; i++)
            {
                if (!pool[i].gameObject.activeSelf)
                {
                    pool[i].gameObject.SetActive( true );
                    return pool[i];
                }
            }

            T obj = spawnNewInstance( this.prefab );
            pool.Add( obj );
            return obj;
        }
    }
}
