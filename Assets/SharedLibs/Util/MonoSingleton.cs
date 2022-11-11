using System;
using UnityEngine;

namespace SharedLibs
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Check to see if we're about to be destroyed
        // private static bool m_ShuttingDown = false;
        // private static object m_Lock = new object();
        private static T m_Instance;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                // Debug.Log(m_ShuttingDown);
                // if (m_ShuttingDown)
                // {
                //     Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                //                      "' already destroyed. Returning null.");
                //     return null;
                // }
                //
                // lock (m_Lock)
                // {
                    if (m_Instance == null)
                    {
                        m_Instance = GetInstance();
                    }
 
                    return m_Instance;
                // }
            }
        }

        private void Awake() {
            m_Instance = GetInstance();
            if (m_Instance)
            {
                DontDestroyOnLoad(gameObject);
            }
            
            Init();
        }

        private static T GetInstance()
        {
            // Search for existing instance.
            var instances = FindObjectsOfType<T>();
 
            // Create new instance if one doesn't already exist.
            if (instances.Length == 0)
            {
                // Need to create a new GameObject to attach the singleton to.
                var singletonObject = new GameObject(typeof(T).Name);
                m_Instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString() + " (Singleton)";
 
                // Make instance persistent.
                DontDestroyOnLoad(singletonObject);
            }
            else if (instances.Length > 1)
            {
                for (int i = 1; i < instances.Length; i++)
                {
                    Destroy(instances[i].gameObject);
                }
            }

            return instances[0];
        }

        /// <summary>
        /// Use this method instead of "Awake", because of DontDestroyOnLoad
        /// </summary>
        public abstract void Init();
 
        // private void OnApplicationQuit()
        // {
        //     m_ShuttingDown = true;
        // }
        
        // private void OnDestroy()
        // {
        //     m_ShuttingDown = true;
        // }
    }
}