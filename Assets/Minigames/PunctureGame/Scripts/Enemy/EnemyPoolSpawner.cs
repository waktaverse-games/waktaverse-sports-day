using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class EnemyPoolSpawner : PoolSpawner<Enemy>
    {
        [SerializeField] private Player player;
        [SerializeField] private FloorEventTrigger floorEvent;
        
        [SerializeField] [Range(0.0f, 10.0f)] private float spawnXRange = 4.0f;
        [SerializeField] [Range(1, 5)] private int spawnYInterval = 2;
        [SerializeField] [ReadOnly] private float spawnYPos = -2.0f;

        private Vector3 SpawnPosition => new Vector3(player.currentPos.x + Random.Range(-spawnXRange, spawnXRange), spawnYPos);

        private void Awake()
        {
            AllocatePool(onCreate: () => {
                var obj = Instantiate(GetBaseObject());
                return obj.GetComponent<Enemy>();
            }, onGet: enemy => {
                enemy.gameObject.SetActive(true);
            }, onRelease: enemy => {
                enemy.gameObject.SetActive(false);
            }, onDestroy: enemy => {
                Destroy(enemy.gameObject);
            });
        }

        private void Start()
        {
            MultipleSpawn();
        }
        
        private void OnEnable()
        {
            floorEvent.onFloorChanged += EnemySpawnOnFloor;
        }

        private void OnDisable()
        {
            floorEvent.onFloorChanged -= EnemySpawnOnFloor;
        }

        private void EnemySpawnOnFloor(int floor)
        {
            if (IsSpawnFloor(floor))
            {
                Spawn();
            }
        }

        private bool IsSpawnFloor(int floor)
        {
            return floor % spawnYInterval == 0;
        }

        public override Enemy Spawn()
        {
            var enemy = Pool.Get();
            enemy.SetPositionState(SpawnPosition);
            spawnYPos -= spawnYInterval;
            return enemy;
        }

        public override void Release(Enemy enemy)
        {
            Pool.Release(enemy);
        }
    }
}