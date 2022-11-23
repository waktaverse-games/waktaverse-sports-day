using UnityEngine;

// https://gist.github.com/zhdlxh48/fcb4b8ff9d5bbbe755c4bba0e4f4af0d

namespace SharedLibs
{
    /// <summary>
    ///     Mono singleton Class. Extend this class to make singleton component.
    ///     Example:
    ///     <code>
    /// public class Foo : MonoSingleton<Foo>
    /// </code>
    ///     . To get the instance of Foo class, use <code>Foo.instance</code>
    ///     Override <code>Init()</code> method instead of using <code>Awake()</code>
    ///     from this class.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        // Query this to see if the singleton has been instantiated anywhere,
        // since querying .Instance will create an instance automatically
        // NOTE: Static field in generic type - the value will be different for each inheritance of MonoSingleton
        public static bool Exists;

        private static T instance;

        public static T Instance
        {
            get
            {
                // Crazy null check to support calling this from outside the main thread
                if (ReferenceEquals(instance, null))
                {
                    instance = FindObjectOfType(typeof(T)) as T;

                    // Object not found - create one
                    if (instance == null)
                    {
#if UNITY_EDITOR
                        //Debug.Log(typeof(T) + " instance was requested and not found. Creating one.");
#endif
                        CreateInstance();
                    }

                    if (instance == null) Debug.LogError("Could not get or create an instance of " + typeof(T));
                }

                return instance;
            }
        }

        // #### UNITY INTERNAL METHODS ####

        // Assign if this is the first instance, else just show a warning that we have more than one
        protected void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                if (instance != null)
                {
                    Exists = true;
                    DontDestroyOnLoad(instance);
                    instance.Init();
                }
                else
                {
                    Debug.LogError("Could not assign the instance of this class (" + typeof(T) + ")");
                }
            }
            else
            {
                Debug.LogWarning("Multiple instances of the MonoSingleton class " + typeof(T) + " created.");
            }
        }

        // #### PROTECTED/PRIVATE METHODS ####

        protected void OnDestroy()
        {
            if (instance != null) instance.SingletonDestroy();
            instance = null;
            Exists = false;
        }


        // #### PUBLIC METHODS ####

        public static void CreateInstance()
        {
            if (instance != null) return;
            instance = new GameObject("Instance of " + typeof(T), typeof(T)).GetComponent<T>();
        }

        // Substitute for Awake() in implementing classes
        public abstract void Init();

        // Called on OnDestroy() for implementing classes to perform any necessary cleanup
        protected virtual void SingletonDestroy()
        {
        }
    }
}