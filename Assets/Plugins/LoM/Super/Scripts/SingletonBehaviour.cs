using System.Collections;
using System.Collections.Generic;
using LoM.Super;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// A singleton MonoBehaviour that inherits from SuperBehaviour.<br/>
    /// Implements a thread-safe singleton pattern as a inheritable base class.
    /// <hr/>
    /// <example>
    /// To create a singleton MonoBehaviour, simply inherit from SingletonBehaviour and add the generic type parameter of the class itself.<br/>
    /// <code>
    /// public class GameManager : SingletonBehaviour&lt;GameManager&gt; 
    /// {
    ///    // ...
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// For a hierarchy of singleton classes, use a generic type parameter T in the base class and inherit from SingletonBehaviour.<br/>
    /// <i>This approach ensures that each subclass (GameManager, UIManager, etc.) functions as a singleton, inheriting the singleton behavior from the base Manager class.</i>
    /// <code>
    /// // Base class with generic type parameter T for singleton management
    /// public class Manager&lt;T&gt; : SingletonBehaviour&lt;T&gt; where T : SuperBehaviour { }
    /// 
    /// // Derived singleton classes
    /// public class GameManager : Manager&lt;GameManager&gt; { }
    /// public class UIManager : Manager&lt;UIManager&gt; { }
    /// </code>
    /// </example>
    /// <hr/>
    /// </summary>
    /// <typeparam name="T">The type of the class to use for the singleton.</typeparam>
    public class SingletonBehaviour<T> : SuperBehaviour where T : SuperBehaviour
    {
        // Private static s_lock
        protected static readonly object s_lock = new object();
        protected static T s_instance;
        
        /// <summary>
        /// Returns if Instance is not null.
        /// </summary>
        public static bool Exists => s_instance != null;
        
        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static T Instance 
        {
            get
            {
                if (s_instance == null)
                {
#if UNITY_6000_0_OR_NEWER
                    T[] instances = FindObjectsByType<T>(FindObjectsSortMode.None);
#else
                    T[] instances = FindObjectsOfType<T>();
#endif
                    if (instances.Length == 1)
                    {
                        s_instance = instances[0];
                    }
                }
                if (s_instance == null)
                {
                    Debug.LogErrorFormat("Trying to access {0} singleton before it is initialized.", typeof(T));
                }
                return s_instance;
            }
        }

        /// <summary>
        /// Awake sets the singleton instance, and should therefore not be overridden or hidden.
        /// </summary>
        private void Awake() 
        {
            lock (s_lock)
            {
                if (s_instance == null) 
                {
                    s_instance = this as T;
                    AfterAwake();
                } 
                else if (s_instance != this)
                {
                    Debug.LogWarningFormat("Trying to create a second instance of {0} [{1}] singleton. Destroying this instance.", typeof(T), gameObject.name);
                    Destroy(gameObject);
                }                
                else
                {
                    AfterAwake();
                }
            }
        }
        
        /// <summary>
        /// Override this method to do something after singleton Awake() is called.
        /// </summary>
        protected virtual void AfterAwake() { }
        
        /// <summary>
        /// OnDestroy destroys the singleton instance, and should therefore not be overridden or hidden.
        /// </summary>
        private void OnDestroy() 
        {
            lock (s_lock)
            {
                if (s_instance == this) 
                {
                    s_instance = null;
                }
            }
            OnAfterDestroy();
        }
        
        /// <summary>
        /// Override this method to do something after singleton OnDestroy() is called.
        /// </summary>
        protected virtual void OnAfterDestroy() { }
    }
}