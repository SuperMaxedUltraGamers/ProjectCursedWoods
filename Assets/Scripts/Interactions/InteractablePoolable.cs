using UnityEngine;

namespace CursedWoods
{
    public class InteractablePoolable : InteractionBase, IPoolObject
    {
        /// <summary>
        /// Reference to this object's pool.
        /// </summary>
        protected IObjectPool myPool;

        public bool IsInUse
        {
            get;
            protected set;
        }

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetActive(false);
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

        public virtual void ReadyUp(IObjectPool myPool)
        {
            this.myPool = myPool;
        }
    }
}