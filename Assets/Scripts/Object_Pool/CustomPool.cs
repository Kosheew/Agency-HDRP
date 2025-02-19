using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class CustomPool<T> where T : Component
    {
        private readonly Queue<T> inactiveQueue;
        private readonly ObjectPool<T> pool;
        private readonly T prefab;
        private readonly HashSet<T> activeObjects;
        private readonly Transform parent;
        
        public CustomPool(T prefab, int maxSize, Transform parent)
        {
            activeObjects = new HashSet<T>();
            this.prefab = prefab;
            this.parent = parent;
            inactiveQueue = new Queue<T>(maxSize);
            
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
            obj.gameObject.SetActive(false);
            activeObjects.Remove(obj);
            inactiveQueue.Enqueue(obj);
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
            /*var script = obj.GetComponent<IPoolable>();
            script.SetPool(this);*/
            return obj;
        }

        public T Get()
        {
            if (inactiveQueue.Count > 0)
            {
                var obj = inactiveQueue.Dequeue();
                pool.Get();
                return obj;
            }
            return pool.Get();
        }

        public void Release(T obj)
        {
            pool.Release(obj);
        }

        public int CountInactive => pool.CountInactive;

        public int CountActive => activeObjects.Count;

        public int PoolSize => pool.CountAll;
    }
}