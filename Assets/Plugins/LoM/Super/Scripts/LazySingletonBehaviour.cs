using System.Collections;
using System.Collections.Generic;
using LoM.Super;
using UnityEngine;

namespace LoM.Super
{
    /// <summary>
    /// A singleton MonoBehaviour that inherits from SuperBehaviour.<br/> 
    /// <i>NOTE: This will create a new instance if one does not exist.</i><br/><br/>
    /// See <see cref="SingletonBehaviour{T}"/> for more information on how to use this class.
    /// </summary>
    /// <typeparam name="T">The type of the singleton.</typeparam>
    public class LazySingletonBehaviour<T> : SingletonBehaviour<T> where T : SuperBehaviour
    {
        /// <summary>
        /// The singleton instance of this class.<br/>
        /// <i>NOTE: This will create a new instance if one does not exist.</i>
        /// </summary>
        public static new T Instance
        {
            get
            {
                lock (s_lock)
                {
                    if (s_instance == null)
                    {
#if UNITY_6000_0_OR_NEWER
                        s_instance = FindAnyObjectByType<T>();
#else
                        s_instance = FindObjectOfType<T>();
#endif
                        if (s_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            s_instance = singleton.AddComponent<T>();
                            singleton.name = $"{typeof(T)} [Singleton]";
                        }
                    }
                    return s_instance;
                }
            }
        }
    }
}