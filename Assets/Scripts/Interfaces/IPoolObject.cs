using UnityEngine;

namespace CursedWoods
{
    public interface IPoolObject
    {
        /// <summary>
        /// Used for object's initialisation and for passing a reference to the object's pool.
        /// </summary>
        /// <param name="myPool">The object's pool that it belongs to, needed when returning the object.</param>
        void ReadyUp(IObjectPool myPool);

        /// <summary>
        /// Is this object already in use.
        /// </summary>
        bool IsInUse { get; }

        /// <summary>
        /// For activating the object after it is get from it's pool, 
        /// also used for resetting object's individual things that are needed for reusing.
        /// </summary>
        /// <param name="pos">Position where the object should be moved to on activation</param>
        /// <param name="rot">Rotation of the object when it is activated.</param>
        void Activate(Vector3 pos, Quaternion rot);

        /// <summary>
        /// For deactivating the object when it is no longer needed and returned to the pool.
        /// </summary>
        void Deactivate();
    }
}