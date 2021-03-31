namespace CursedWoods
{
    public interface IObjectPool
    {
        /// <summary>
        /// Populates the pool with objects.
        /// </summary>
        void CreateObjects();

        /// <summary>
        /// Used to get objects from this pool.
        /// </summary>
        /// <returns>The passed object from this pool.</returns>
        IPoolObject GetObject();

        /// <summary>
        /// Returns the object to this pool.
        /// </summary>
        /// <param name="obj">The object which is returned to this pool.</param>
        void ReturnObject(IPoolObject obj);

        /// <summary>
        /// Clears the pool from all the objects.
        /// </summary>
        void ClearPool();
    }
}