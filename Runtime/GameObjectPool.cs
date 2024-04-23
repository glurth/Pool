using UnityEngine;
using System;
namespace EyE.Collections
{
    /// <summary>
    /// This variant of Pool<typeparamref name="T"/> works on unity GameObjects.  
    /// When new elements are created for the pool, it will use the defaultPoolValue as a preFabTemplace to instantiate a new version (as a child of the instantiationParent member).
    /// The option to Activate and Deactivate the pool element's GameObject, when Pulled or Tossed is provided via the SetActiveOnPullDeactiveOnToss member.
    /// Callback functions, invoked when objects are pulled or tossed from the pool can, optionally, be passed in during construction.
    /// </summary>
    /// <typeparam name="T">A type of component added to unity GameObjects</typeparam>
    public class GameObjectPool : Pool<GameObject>
    {
        public bool SetActiveOnPullDeactiveOnToss = true;
        public Transform instantiationParent;

        /// <summary>
        /// populates the parameter with a new instance of a <typeparamref name="T"/>
        /// </summary>
        /// <param name="value">the calling function will have the param filled.</param>
        protected override void AssignDefaultValueTo(out GameObject value)
        {
            if (defaultPoolValue == null)
                throw new System.NullReferenceException(GetType().Name + " has not been assigned a defaultPoolValue, and so cannot create new ones based on it.");
            value = Component.Instantiate<GameObject>(defaultPoolValue, instantiationParent);
        }

        void OnMonoPull(GameObject pulledObject)
        {
            if (SetActiveOnPullDeactiveOnToss)
                pulledObject.SetActive(true);
        }
        void OnMonoToss(GameObject objectPutBackInPool)
        {
            if (SetActiveOnPullDeactiveOnToss)
                objectPutBackInPool.SetActive(false);
        }

        /// <summary>
        /// It is recommended that you use the 2(or more) parameter versions of MonoPool constructors to properly initialize for Unity.
        /// A warning will be generated in the console if this version is used, but it will otherwise try to proceed as normal.
        /// </summary>
        public GameObjectPool() : base()
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
        public GameObjectPool(GameObject defaultPoolValue, Transform instantiationParent) : base(defaultPoolValue)
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
        /// <param name="onPull">Action that will be invoked when an object is pulled from the pool. It will be passed the object that was just pulled.</param>
        /// <param name="onToss">Action that will be invoked when an object is tossed into the pool. It will be passed the object that was just tossed.</param>
        public GameObjectPool(GameObject defaultPoolValue, Transform instantiationParent, Action<GameObject> onPull, Action<GameObject> onToss) : base(defaultPoolValue) //note we don't call the base version that takes action params- we'll create our own here.
        {
            this.instantiationParent = instantiationParent;
            this.onPull = (a) => { OnMonoPull(a); onPull(a); };
            this.onToss = (a) => { OnMonoToss(a); onToss(a); };
        }
    }
}