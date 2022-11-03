using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace GameHeaven.PunctureGame {
    public class EnemyManager : MonoBehaviour {
        [SerializeField] private GameObject enemyPrefab;

        [Header("Spawn Value")]
        [SerializeField] private int startSpawnCount = 6;
        [SerializeField] private float spawnHorizontalRange = 4.0f;
        [SerializeField] [ReadOnly] private int spawnYNext = -2;
        [SerializeField] private int spawnYInterval = 2;

        [Header("Components")]
        [SerializeField] private PlayerController player;
        [SerializeField] private PlayerFloorChecker floorChecker;

        private ObjectPool<EnemyController> enemyPool;

        private void Awake() {
            enemyPool = new ObjectPool<EnemyController>(
                () => {
                    var enemy = Create();
                    return enemy;
                },
                enemy => {
                    enemy.onRelease += enemyPool.Release;
                    enemy.Spawn(RandSpawnPoint());
                    enemy.gameObject.SetActive(true);
                },
                enemy => {
                    enemy.gameObject.SetActive(false);
                    enemy.onRelease -= enemyPool.Release;
                },
                enemy => { Destroy(enemy.gameObject); },
                maxSize: 30);
        }

        private void Start() {
            for (var i = 0; i < startSpawnCount; i++) enemyPool.Get();
        }

        private void OnEnable() {
            floorChecker.onFloorChanged += OnPlayerFloorChanged;
        }

        private void OnDisable() {
            floorChecker.onFloorChanged -= OnPlayerFloorChanged;
        }

        public EnemyController Create() {
            var obj = Instantiate(enemyPrefab);
            return obj.GetComponent<EnemyController>();
        }

        private Vector3 RandSpawnPoint() {
            var pos = Vector3.zero;
            pos.x += player.currentPos.x + Random.Range(-spawnHorizontalRange, spawnHorizontalRange);
            pos.y = spawnYNext;
            spawnYNext -= spawnYInterval;
            return pos;
        }

        private void OnPlayerFloorChanged(int floor) {
            if (floor % spawnYInterval == 0) enemyPool.Get();
        }

        // TODO: 적들 Pool에 되돌리기 및 리스폰
    }
}