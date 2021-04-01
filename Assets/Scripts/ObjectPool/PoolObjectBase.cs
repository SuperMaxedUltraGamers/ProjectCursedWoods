using UnityEngine;

namespace CursedWoods
{
    public class PoolObjectBase : MonoBehaviour, IPoolObject
    {
        #region Protected fields

        /// <summary>
        /// Reference to this object's pool.
        /// </summary>
        protected IObjectPool myPool;

        #endregion Protected fields

        #region Properties

        public bool IsInUse
        {
            get;
            protected set;
        }

        #endregion Properties

        #region Public API

        public virtual void ReadyUp(IObjectPool myPool)
        {
            this.myPool = myPool;
        }

        public virtual void Activate(Vector3 pos, Quaternion rot)
        {
            IsInUse = true;
            transform.position = pos;
            transform.rotation = rot;
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            IsInUse = false;
            myPool.ReturnObject(this);
            gameObject.SetActive(false);
        }

        #endregion Public API
    }
}