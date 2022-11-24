using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public abstract class FloorSpawner<T> : MonoBehaviour where T : Component
    {
        [SerializeField] [Required] protected GenericPoolManager<T> poolManager;

        [SerializeField] [Required] protected FloorManager floorManager;

        [SerializeField] private int startSpawnCount;

        [SerializeField] private bool isRandomSpawn;

        private int sequenceIndex;

        public bool IsRandomSpawn
        {
            get => isRandomSpawn;
            protected set => isRandomSpawn = value;
        }

        private int SequenceIndex
        {
            set => sequenceIndex = sequenceIndex >= poolManager.PoolVariationCount - 1 ? 0 : value;
            get => sequenceIndex;
        }

        private int RandomPoolIndex => Random.Range(0, poolManager.PoolVariationCount);

        public virtual void Start()
        {
            for (var i = 0; i < startSpawnCount; i++) Spawn();
        }

        protected virtual void OnEnable()
        {
            floorManager.OnFloorChanged += SpawnByFloor;
        }

        protected virtual void OnDisable()
        {
            floorManager.OnFloorChanged -= SpawnByFloor;
        }

        public abstract void SpawnByFloor(int floor);
        public abstract T Spawn();
        public abstract void Release(T obj);

        public T GetInstance()
        {
            return IsRandomSpawn
                ? poolManager.GetRandomFromPool() ?? poolManager.GetFromPool(RandomPoolIndex)
                : poolManager.GetFromPool(SequenceIndex++);
        }
    }
}