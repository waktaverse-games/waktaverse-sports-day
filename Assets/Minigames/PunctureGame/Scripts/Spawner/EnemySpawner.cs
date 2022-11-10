using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace GameHeaven.PunctureGame
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject baseEnemyObject;
        
        private ObjectPool<Enemy> enemyPool;
        [SerializeField] [DisableInPlayMode] private int maxPoolSize = 30;
        
        [SerializeField] private int startSpawnCount = 6;

        [SerializeField] private float spawnHorizontalRange = 4.0f;
        [SerializeField] [ReadOnly] private int spawnYNext = -2;
        [SerializeField] private int spawnYInterval = 2;

        private List<Enemy> activatedEnemies;

        [SerializeField] private Player player;
        [SerializeField] private FloorEventTrigger floorEventTrigger;

        private void Awake()
        {
            enemyPool = new ObjectPool<Enemy>(
                createFunc: () =>
                {
                    var obj = Instantiate(baseEnemyObject);
                    return obj.GetComponent<Enemy>();
                },
                actionOnGet: enemy =>
                {
                    activatedEnemies.Add(enemy);
                    enemy.OnRelease += enemyPool.Release;
                    enemy.Spawn();
                },
                actionOnRelease: enemy =>
                {
                    activatedEnemies.Remove(enemy);
                    enemy.OnRelease -= enemyPool.Release;
                    enemy.gameObject.SetActive(false);
                },
                actionOnDestroy: enemy =>
                {
                    activatedEnemies.Remove(enemy);
                    Destroy(enemy.gameObject);
                },
                maxSize: maxPoolSize);

            activatedEnemies = new List<Enemy>();
        }
        
        private void Start()
        {
            for (var i = 0; i < startSpawnCount; i++)
            {
                SpawnEnemy();
            }
        }
        
        private void OnEnable()
        {
            floorEventTrigger.onFloorChanged += EnemySpawnOnFloorChanged;
        }

        private void OnDisable()
        {
            floorEventTrigger.onFloorChanged -= EnemySpawnOnFloorChanged;
        }

        private void EnemySpawnOnFloorChanged(int floor)
        {
            if (floor % spawnYInterval == 0)
            {
                SpawnEnemy();
            }
        }

        public Enemy SpawnEnemy()
        {
            var enemy = enemyPool.Get();
            var point = GetRandomSpawnPoint();
            enemy.Init(point);
            return enemy;
        }
        private Vector3 GetRandomSpawnPoint()
        {
            var pos = Vector3.zero;
            pos.x += player.currentPos.x + Random.Range(-spawnHorizontalRange, spawnHorizontalRange);
            pos.y = spawnYNext;
            print("Spawn: " + pos);
            spawnYNext -= spawnYInterval;
            return pos;
        }

        public void ReleaseEnemy(Enemy enemy)
        {
            enemyPool.Release(enemy);
        }

        public List<Enemy> GetActiveEnemies()
        {
            return activatedEnemies;
        }
    }
}