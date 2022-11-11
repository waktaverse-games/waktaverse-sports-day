using SharedLibs.Score;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class ItemPoolSpawner : PoolSpawner<Item>
    {
        [SerializeField] private Player player;
        [SerializeField] private FloorEventTrigger floorEvent;
        
        [SerializeField] [Range(0.0f, 10.0f)] private float spawnXRange = 4.0f;
        [SerializeField] [ReadOnly] private float spawnYPos = -2.0f;
        [SerializeField] [Range(1, 5)] private int spawnYMinInterval = 2;
        [SerializeField] [Range(1, 5)] private int spawnYMaxInterval = 4;
        
        [SerializeField] private float spawnTriggerDist = 5.0f;
        
        private Vector3 SpawnPosition => new Vector3(player.currentPos.x + Random.Range(-spawnXRange, spawnXRange), spawnYPos);

        private int NextSpawnYIntervalDist => Random.Range(spawnYMinInterval, spawnYMaxInterval + 1);

        private void Awake()
        {
            AllocatePool(onCreate: () => {
                var obj = Instantiate(GetBaseObject());
                return obj.GetComponent<Item>();
            }, onGet: item => {
                item.OnRelease += Release;
                item.gameObject.SetActive(true);
            }, onRelease: item => {
                item.OnRelease -= Release;
                item.gameObject.SetActive(false);
            }, onDestroy: item => {
                Destroy(item.gameObject);
            });
        }

        private void Start()
        {
            MultipleSpawn();
        }
        
        private void OnEnable()
        {
            floorEvent.onFloorChanged += ItemSpawnByDistance;
        }

        private void OnDisable()
        {
            floorEvent.onFloorChanged -= ItemSpawnByDistance;
        }

        private void ItemSpawnByDistance(int floor)
        {
            if (IsSpawnRange())
            {
                Spawn();
            }
        }

        private bool IsSpawnRange()
        {
            return player.currentPos.y - spawnTriggerDist <= spawnYPos;
        }

        public override Item Spawn()
        {
            var item = Pool.Get();
            
            var itemTf = item.transform;
            itemTf.position = SpawnPosition;
            spawnYPos -= NextSpawnYIntervalDist;
            
            return item;
        }

        public override void Release(Item item)
        {
            Pool.Release(item);
        }
    }
}