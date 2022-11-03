using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace GameHeaven.PunctureGame
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private EnemyFactory factory;
        private ObjectPool<Enemy> enemyPool;
        
        [SerializeField] private int startSpawnCount = 6;
        [SerializeField] private float spawnHorizontalRange = 4.0f;
        [SerializeField, ReadOnly] private int spawnYNext = -1;
        [SerializeField] private int spawnYInterval = 2;

        [SerializeField] private PlayerController player;
        [SerializeField] private PlayerFloorChecker floorChecker;

        private void Awake()
        {
            enemyPool = new ObjectPool<Enemy>(
                createFunc: () =>
                {
                    var enemy = factory.Create();
                    enemy.enemyPool = enemyPool;
                    enemy.playerController = player;
                    return enemy;
                },
                actionOnGet: enemy =>
                {
                    enemy.gameObject.SetActive(true);
                    var pos = transform.position;
                    pos.x += player.currentPos.x + Random.Range(-spawnHorizontalRange, spawnHorizontalRange);
                    pos.y = spawnYNext;
                    spawnYNext -= spawnYInterval;
                    var isLeft = Random.Range(0, 2) == 1;
                    enemy.Spawn(pos, isLeft);
                    Debug.Log($"Spawned - Pos: {pos} / Left: {isLeft}");
                },
                actionOnRelease: enemy =>
                {
                    enemy.gameObject.SetActive(false);
                },
                actionOnDestroy: enemy =>
                {
                    Destroy(enemy.gameObject);
                }, 
                maxSize: 30);
        }

        private void Start()
        {
            for (int i = 0; i < startSpawnCount; i++)
            {
                enemyPool.Get();
            }
        }

        private void OnEnable()
        {
            floorChecker.onFloorChanged += OnPlayerFloorChanged;
        }

        private void OnDisable()
        {
            floorChecker.onFloorChanged -= OnPlayerFloorChanged;
        }

        private void OnPlayerFloorChanged(int floor)
        {
            Debug.Log("Player entered on " + floor + " floor");
            if (floor % spawnYInterval == 0)
            {
                enemyPool.Get();
            }
        }
        
        // TODO: 적들 Pool에 되돌리기 및 리스폰
    }
}