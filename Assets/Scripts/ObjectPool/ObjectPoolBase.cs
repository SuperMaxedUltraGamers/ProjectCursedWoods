using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class ObjectPoolBase<T> : MonoBehaviour, IObjectPool
        where T : Object, IPoolObject
    {
        #region Private fields

        /// <summary>
        /// This pool's object prefab.
        /// </summary>
        [SerializeField]
        private T prefab = null;

        /// <summary>
        /// The pool where the objects are.
        /// </summary>
        private Queue<T> pool = new Queue<T>();

        #endregion Private fields

        #region Protected fields

        /// <summary>
        /// The amount of objects that are created at the start.
        /// </summary>
        [SerializeField, Range(0, 50)]
        protected int poolStartSize = 5;

        #endregion Protected fields

        #region Public API

        public void CreateObjects()
        {
            // Instantiate objects for wanted amount, place them in the pool and initialise them.
            for (int i = 0; i < poolStartSize; i++)
            {
                IPoolObject newObj = Instantiate(prefab);
                pool.Enqueue((T)newObj);
                newObj.ReadyUp(this);
            }
        }

        public IPoolObject GetObject()
        {
            IPoolObject obj;

            // Get object from the pool or instantiate a new one if there are no objects left inside the pool.
            if (pool.Count > 0)
            {
                obj = pool.Dequeue();
                return obj;
            }
            else
            {
                obj = Instantiate(prefab);
                obj.ReadyUp(this);
                return obj;
            }
        }

        public void ReturnObject(IPoolObject obj)
        {
            // Queue the object back to this pool.
            pool.Enqueue((T)obj);
        }

        #endregion Public API
    }
}