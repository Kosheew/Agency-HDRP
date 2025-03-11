using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class CustomPool<T> where T : Component
    {
        private readonly ObjectPool<T> pool;
        private readonly T prefab;
        private readonly HashSet<T> activeObjects;
        private readonly Transform parent;
        
        public CustomPool(T prefab, int maxSize, Transform parent)
        {
            activeObjects = new HashSet<T>();
            this.prefab = prefab;
            this.parent = parent;
            
            pool = new ObjectPool<T>(
                OnCreateObject,
                OnGet,
                OnRelease,
                OnDestroy,
                maxSize: maxSize
            );
        }

        private void OnDestroy(T obj)
        {
            Object.Destroy(obj.gameObject);
        }

        private void OnRelease(T obj)
        {
            activeObjects.Remove(obj);
            obj.gameObject.SetActive(false);
        }

        private void OnGet(T obj)
        {
            obj.gameObject.SetActive(true);
            activeObjects.Add(obj);
        }

        private T OnCreateObject()
        {
            var obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            return obj;
        }

        public T Get()
        {
            return pool.Get();
        }

        public void Release(T obj)
        {
            if (activeObjects.Contains(obj))
            {
                pool.Release(obj);
            }
        }

        public int CountInactive => pool.CountInactive;

        public int CountActive => activeObjects.Count;

        public int PoolSize => pool.CountAll;
    }
}