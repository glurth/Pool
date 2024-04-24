using System.Collections.Generic;
using System;

namespace EyE.Collections
{
    /// <summary>
    ///This is an implementation of a generic object pool. An object pool is a container that holds a collection of objects that can be reused, rather than creating a new instance each time an object is needed. This can be useful for improving performance and reducing memory usage, as it can be more efficient to reuse objects rather than creating new ones.
    ///The class has several methods for adding objects back to the pool (Toss), and retrieving objects from the pool (Pull). It also has two delegate properties onPull and onToss, which are called each time an object is retrieved from or added back to the pool. The class can be initialized with a default value for objects in the pool, and this default value will be used if the pool is empty when an object is requested.
    ///The class implements a list to store the objects in the pool, and it uses the RemoveAt method to retrieve the last item in the list each time an object is requested, ensuring that the most recently added object is used first.
    ///Overall, this implementation provides a basic object pool implementation that can be customized by defining custom actions for onPull and onToss and by providing a default value for objects in the pool.
    /// </summary>
    /// <typeparam name="T">The type of object stored in the pool.</typeparam>
    public class Pool<T>
    {
        /// <summary>
        /// The default value of new items pulled from the pool.
        /// </summary>
        protected T defaultPoolValue;

        /// <summary>
        /// The action to be performed when an item is pulled from the pool.
        /// </summary>
        protected Action<T> onPull = (T) => { };

        /// <summary>
        /// The action to be performed when an item is tossed back into the pool.
        /// </summary>
        protected Action<T> onToss = (T) => { };

        /// <summary>
        /// The list of contents in the pool.  These are unused items, doing nothing but waiting to be pulled.
        /// </summary>
        protected List<T> poolContents = new List<T>();

        /// <summary>
        /// The pool will keep a record of all items that have been pulled from it.  Invoking TossAll() will toss them all back into the pool.
        /// Keeping this option true will increase the time required to perform pull and toss operations.
        /// Setting this option to true, after pool operation starts is not recommended.
        /// </summary>
        protected bool recordPulled = true;

        /// <summary>
        /// internally stores items previously pulled from the pool.  Used by TossAllBack, but requires updating every pull/toss.
        /// </summary>
        protected List<T> pulledList = new List<T>();

        /// <summary>
        /// Constructor the specifies if pull objects should be recorded
        /// </summary>
        /// <param name="recordPulled">should pulled objects be recorded? (required for TossAllBack() )</param>
        public Pool(bool recordPulled = true) { defaultPoolValue = default(T); this.recordPulled = recordPulled; }

        /// <summary>
        /// Sets the default pool value.  Changing the after usage of the pool has started is possible, but pull results may be unpredictable.
        /// </summary>
        /// <param name="defaultPoolValue">The new default pool value</param>
        public void SetDefault(T defaultPoolValue) { this.defaultPoolValue = defaultPoolValue; }

        /// <summary>
        /// Constructor that sets the default pool value.
        /// </summary>
        /// <param name="defaultPoolValue">The default pool value</param>
        /// <param name="recordPulled">should pulled objects be recorded? (required for TossAllBack() )</param>
        public Pool(T defaultPoolValue, bool recordPulled=true) { this.defaultPoolValue = defaultPoolValue; this.recordPulled = recordPulled; }

        /// <summary>
        /// Constructor that sets the default pool value, the on pull action and the on toss action.
        /// </summary>
        /// <param name="defaultPoolValue">The default pool value</param>
        /// <param name="onPull">The action to be performed when an item is pulled from the pool</param>
        /// <param name="onToss">The action to be performed when an item is tossed back into the pool</param>
        /// <param name="recordPulled">should pulled objects be recorded? (required for TossAllBack() )</param>
        public Pool(T defaultPoolValue, Action<T> onPull, Action<T> onToss, bool recordPulled=true)
        {
            this.defaultPoolValue = defaultPoolValue;
            this.recordPulled = recordPulled;
            this.onPull = onPull;
            this.onToss = onToss;
        }

        /// <summary>
        /// Tosses an item back into the pool.
        /// </summary>
        /// <param name="valueToPutBackInPool">The item to be tossed back into the pool</param>
        public void Toss(T valueToPutBackInPool)
        {
            poolContents.Add(valueToPutBackInPool);
            if (recordPulled)
                pulledList.Remove(valueToPutBackInPool);
            onToss(valueToPutBackInPool);
        }

        /// <summary>
        /// Tosses a list of items back into the pool.
        /// </summary>
        /// <param name="valueToPutBackInPool">The list of items to be tossed back into the pool</param>
        public void Toss(IList<T> valueToPutBackInPool)
        {
            foreach (T element in valueToPutBackInPool)
            {
                Toss(element);
            }
        }


        /// <summary>
        /// Pulls an item from the pool.
        /// </summary>
        /// <returns>The item pulled from the pool</returns>
        public T Pull()
        {
            T returnValue;
            if (poolContents.Count != 0)
            {
                returnValue = poolContents[poolContents.Count - 1];
                poolContents.RemoveAt(poolContents.Count - 1);
            }
            else
            {
                AssignDefaultValueTo(out returnValue);
            }
            onPull(returnValue);
            if (recordPulled)
                pulledList.Add(returnValue);
            return returnValue;
        }

        /// <summary>
        /// This function can be overridden in descendants to specify how new objects that don't yet exist in the pool should be initialized.
        /// </summary>
        /// <param name="value">this object will be filled with, or set to a reference to, new objects when they are pulled from the pool</param>
        protected virtual void AssignDefaultValueTo(out T value)
        {
            value = defaultPoolValue;
        }

        /// <summary>
        /// Function used to pull a number of items from the pool
        /// </summary>
        /// <param name="count">number of items to pull from the poll</param>
        /// <param name="addTo">a list that these pulled items will be appended to</param>
        public void Pull(int count, IList<T> addTo)
        {
            for (int i = 0; i < count; i++)
            {
                addTo.Add(Pull());
            }
        }

        /// <summary>
        /// This function will only work if recordPulled is enabled.  Will throw an expection if invoked without it. 
        /// </summary>
        public void TossAllBack()
        {
            if (!recordPulled) throw new System.ArgumentException("TossAllBack has been invoked on a Pool, but this Pool has the recordPull member set to false.");
            foreach (T element in pulledList)
            {
                Toss(element);
            }
            pulledList.Clear();
        }
    }

    
}

