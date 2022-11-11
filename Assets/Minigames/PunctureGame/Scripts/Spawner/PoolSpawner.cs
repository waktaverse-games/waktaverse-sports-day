using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    public abstract class PoolSpawner<T> : MonoBehaviour, ISpawner<T> where T : MonoBehaviour
    {
        [SerializeField] private GameObject[] baseObjs;
        
        private ObjectPool<T> pool;
        [SerializeField] private int maxPoolSize;
        [SerializeField] private int[] objEachCounts;

        [SerializeField] private int startSpawnCount = 0;

        [SerializeField] private List<T> activeElements;

        [SerializeField] [DisableInPlayMode] private bool isRandomSpawn = true;

        public ObjectPool<T> Pool => pool;

        public void AllocatePool(Func<T> onCreate, Action<T> onGet, Action<T> onRelease, Action<T> onDestroy)
        {
            activeElements = new List<T>();
            
            onGet += elem => activeElements.Add(elem);
            onRelease += elem => activeElements.Remove(elem);
            onDestroy += elem => activeElements.Remove(elem);
            
            pool = new ObjectPool<T>(
                createFunc: onCreate,
                actionOnGet: onGet,
                actionOnRelease: onRelease,
                actionOnDestroy: onDestroy,
                maxSize: maxPoolSize);
        }

        private int curSeqIndex = 0;
        public GameObject GetBaseObject()
        {
            return isRandomSpawn ? GetBaseObjectRandomly() : GetBaseObjectSequentially();
        }

        private GameObject GetBaseObjectSequentially()
        {
            var obj = baseObjs[curSeqIndex++];
            curSeqIndex = curSeqIndex >= baseObjs.Length ? 0 : curSeqIndex;
            return obj;
        }
        private GameObject GetBaseObjectRandomly()
        {
            return baseObjs[Random.Range(0, baseObjs.Length)];
        }

        public List<T> MultipleSpawn(int count = -1)
        {
            count = count == -1 ? startSpawnCount : count;

            var elems = new List<T>();
            for (var i = 0; i < count; i++) elems.Add(Spawn());

            return elems;
        }

        public List<T> GetActiveElements()
        {
            return activeElements;
        }

        public abstract T Spawn();
        public abstract void Release(T elem);
    }
}