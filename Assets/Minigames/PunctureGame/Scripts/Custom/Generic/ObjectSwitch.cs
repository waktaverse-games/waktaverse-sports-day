using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    [System.Serializable]
    public struct SwitchPair<TKey, TValue> where TKey : System.Enum
    {
        [SerializeField] private TKey key;
        [SerializeField] private TValue value;

        public TKey Key => key;
        public TValue Value => value;

        public KeyValuePair<TKey, TValue> ToKeyValuePair() => new (Key, Value);
    }
    
    public class ObjectSwitch<TKey> : MonoBehaviour where TKey : System.Enum
    {
        [SerializeField] private List<SwitchPair<TKey, GameObject>> switchList;
        // private Dictionary<TKey, GameObject> switchDic = new();
        //
        // protected virtual void Awake()
        // {
        //     switchDic = new Dictionary<TKey, GameObject>(switchList.Select((sPair) =>
        //         new KeyValuePair<TKey, GameObject>(sPair.Key, sPair.Value)));
        // }

        public event Action<GameObject> OnEnable;
        public event Action<GameObject> OnDisable;

        public GameObject Enable(TKey key)
        {
            foreach (var pair in switchList)
            {
                Disable(pair.Value);
            }
            return ForceEnable(key);
        }

        public GameObject ForceEnable(TKey key)
        {
            var obj = switchList.Find((s) => s.Key.Equals(key)).Value;
            obj.SetActive(true);
            OnEnable?.Invoke(obj);
            return obj;
        }

        public void Disable(TKey key)
        {
            Disable(switchList.Find((s) => s.Key.Equals(key)).Value);
        }
        private void Disable(GameObject obj)
        {
            OnDisable?.Invoke(obj);
            obj.SetActive(false);
        }
    }
}