using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public abstract class DisposableSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T Instance => _instance != null ? _instance : GetInstance();

        private void Awake()
        {
            _instance = this as T;
            
            RenameSingletonObject();
            Initialize();
        }

        private static T GetInstance()
        {
            var instances = FindObjectsOfType<T>();
            switch (instances.Length)
            {
                case > 1:
                    Debug.LogErrorFormat("There are {0} instance.\nPlease remove the remaining objects.",
                        instances.Length);
                    break;
                case 0:
                    Debug.LogErrorFormat(
                        "You are trying to access instance that is not exist.\nYou have to singleton create.");
                    break;
            }
            _instance = instances[0];

            return _instance;
        }

        private void RenameSingletonObject()
        {
            var o = this.gameObject;
            o.name = "[Singleton] " + o.name;
        }

        protected abstract void Initialize();
    }
}