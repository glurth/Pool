﻿using UnityEngine;
using System;
namespace EyE.Collections
{
    /// <summary>
    /// This variant of Pool<typeparamref name="T"/> works on MonoBehaviour derived classes.  
    /// When new elements are created for the pool, it will use the defaultPoolValue as a preFabTemplace to instantiate a new version (as a child of the instantiationParent member).
    /// The option to enable and disable the pool element, when Pulled or Tossed is provided via the enableOnPullDisenableOnToss member.
    /// Note: this class does not affect the active-state of the GameObject.
    /// </summary>
    /// <typeparam name="T">A type of component added to unity GameObjects</typeparam>
    public class MonoPool<T> : Pool<T> where T : MonoBehaviour
    {
        public bool enableOnPullDisenableOnToss = true;
        public Transform instantiationParent;

        /// <summary>
        /// populates the parameter with a new instance of a <typeparamref name="T"/>
        /// </summary>
        /// <param name="value">the calling function will have the param filled.</param>
        protected override void AssignDefaultValueTo(out T value)
        {
            if (defaultPoolValue == null)
                throw new System.NullReferenceException(GetType().Name + " has not been assigned a defaultPoolValue, and so cannot create new ones based on it.");
            value = Component.Instantiate<T>(defaultPoolValue, instantiationParent);
        }

        void OnMonoPull(T pulledObject)
        {
            if (enableOnPullDisenableOnToss)
                pulledObject.enabled=true;
        }
        void OnMonoToss(T objectPutBackInPool)
        {
            if (enableOnPullDisenableOnToss)
                objectPutBackInPool.enabled = false;
        }

        /// <summary>
        /// It is recommended that you use the 2(or more) parameter versions of MonoPool constructors to properly initialize for Unity.
        /// A warning will be generated in the console if this version is used, but it will otherwise try to proceed as normal.
        /// </summary>
        public MonoPool() : base()
        {
            Debug.LogWarning("You are using the paramaterless constructor on a MonoPool. The defaultPoolValue will remain null.\nUse the MonoPool(T defaultPoolValue, Transform instantiationParent) version instead.\nIf you manually set these value later, this warning can be ignored");
            this.onPull = (a) => { OnMonoPull(a); };
            this.onToss = (a) => { OnMonoToss(a); };
        }

        /// <summary>
        /// Used to create a new MonoPool object.  It takes parameters that specify how new elements, that don't exist yet, should be created.
        /// </summary>
        /// <param name="defaultPoolValue">This component reference will be used as a "preFab" to Instantiate new pool elements into the scene.</param>
        /// <param name="instantiationParent">A Transform that specifies the parent of any newly created elements.</param>
        public MonoPool(T defaultPoolValue, Transform instantiationParent) : base(defaultPoolValue)
        {
            this.instantiationParent = instantiationParent;
            this.onPull = (a) => { OnMonoPull(a); };
            this.onToss = (a) => { OnMonoToss(a); };
        }


        /// <summary>
        /// Used to create a new MonoPool object.  It takes parameters that specify how new elements, that don't exist yet, should be created.
        /// Provides parameters so actions can be added 
        /// </summary>
        /// <param name="defaultPoolValue">This component reference will be used as a "preFab" to Instantiate new pool elements into the scene.</param>
        /// <param name="instantiationParent">A Transform that specifies the parent of any newly created elements.</param>
        /// <param name="onPull">Action that will be invoked when an object is pulled from the pool.</param>
        /// <param name="onToss">Action that will be invoked when an object is tossed into the pool.</param>
        public MonoPool(T defaultPoolValue, Transform instantiationParent, Action<T> onPull, Action<T> onToss) : base(defaultPoolValue)
        {
            this.instantiationParent = instantiationParent;
            this.onPull = (a) => { OnMonoPull(a); onPull(a); };
            this.onToss = (a) => { OnMonoToss(a); onToss(a); };
        }
    }
}