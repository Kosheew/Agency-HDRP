using UnityEngine;

namespace ObjectPool
{
   public interface IPoolable
   {
      public void SetPool<T>(CustomPool<T> pool) where T : Component;
   }
}