using System;
using System.Collections.Generic;
using System.Linq;
using Redcode.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    [Serializable]
    internal struct PoolData<T> where T : Component
    {
        [SerializeField] private string name;
        [SerializeField] private T component;
        [SerializeField] [Min(0)] private int count;
        [SerializeField] private Transform container;
        [SerializeField] private bool nonLazy;
        [SerializeField] [Min(0)] private int weight;

        public string Name => name;
        public T Component => component;
        public int Count => count;
        public Transform Container => container;
        public bool NonLazy => nonLazy;
        public int Weight => weight;
    }

    // NOTE: ScriptExecutionOrder set to -1
    public abstract class GenericPoolManager<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private List<PoolData<T>> pools;
        private readonly List<T> _activeObjects = new();

        private readonly List<IPool<T>> _poolsObjects = new();
        private readonly List<int> _poolWeights = new();

        private int totalWeight;

        public int PoolVariationCount => _poolsObjects.Count;

        private void Awake()
        {
            var typeGroups = pools.Select(p => p.Name).GroupBy(n => n).Where(g => g.Count() > 1);

            if (typeGroups.Count() > 0)
                throw new Exception(
                    $"Pool Manager already contains pool with Enum Type \"{typeGroups.First().Select(g => g).First()}\"");

            foreach (var poolData in pools)
            {
                var pool = Pool.Create(poolData.Component, poolData.Count,
                    poolData.Container ? poolData.Container : transform);
                _poolsObjects.Add(poolData.NonLazy ? pool.NonLazy() : pool);

                _poolWeights.Add(poolData.Weight);
                totalWeight += poolData.Weight;
            }
        }

        #region Get from pool

        public List<T> GetActiveObjects()
        {
            return _activeObjects;
        }

        #endregion

        #region Get pool

        public IPool<T> GetPool(int index)
        {
            return _poolsObjects[index];
        }

        public IPool<T> GetPool(string name)
        {
            return _poolsObjects[pools.FindIndex(p => p.Name.Equals(name))];
        }

        #endregion

        #region Get from pool

        public T GetFromPool(int index)
        {
            var obj = GetPool(index).Get();
            _activeObjects.Add(obj);
            return obj;
        }

        public T GetFromPool(string name)
        {
            var obj = GetPool(name).Get();
            _activeObjects.Add(obj);
            return obj;
        }

        public T GetRandomFromPool()
        {
            var randNum = Random.Range(0, totalWeight) + 1;

            var weightSum = 0;
            var poolIndex = 0;
            while (poolIndex < _poolWeights.Count)
            {
                var weight = _poolWeights[poolIndex];
                if (weightSum < randNum && randNum <= weightSum + weight) break;
                weightSum += weight;
                poolIndex++;
            }

            var obj = GetPool(poolIndex == _poolWeights.Count ? Random.Range(0, PoolVariationCount) : poolIndex).Get();
            _activeObjects.Add(obj);
            return obj;
        }

        #endregion

        #region Take to pool

        public void TakeToPool(int index, T component)
        {
            _activeObjects.Remove(component);
            _poolsObjects[index].Take(component);
        }

        public void TakeToPool(string name, T component)
        {
            _activeObjects.Remove(component);
            GetPool(name).Take(component);
        }

        #endregion
    }
}