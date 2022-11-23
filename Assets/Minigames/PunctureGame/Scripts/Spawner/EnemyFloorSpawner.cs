using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    public class EnemyFloorSpawner : FloorSpawner<Enemy>
    {
        [SerializeField] [Required] private PlayerController controller;

        [SerializeField] [Min(1)] private int startSpawnFloor;
        [SerializeField] [Min(1)] private int spawnFloorInterval;

        [SerializeField] private IntRange spawnXRange;
        [SerializeField] [ReadOnly] private float spawnYPos;

        [SerializeField] private ScoreCollector scoreCollector;

        private Vector3 SpawnPosition =>
            new(controller.Position.x + Random.Range(spawnXRange.Min, spawnXRange.Max), spawnYPos);

        private void Awake()
        {
            controller = FindObjectOfType<PlayerController>();
        }

        public override void SpawnByFloor(int floor)
        {
            if (floor % spawnFloorInterval == 0 && floor >= startSpawnFloor) Spawn();
        }

        public override Enemy Spawn()
        {
            var enemy = GetInstance();
            spawnYPos -= spawnFloorInterval;
            if (enemy)
            {
                enemy.OnTrodden += scoreCollector.AddEnemyScore;
                enemy.SetPositionState(SpawnPosition);
            }

            return enemy;
        }

        public override void Release(Enemy enemy)
        {
            enemy.OnTrodden -= scoreCollector.AddEnemyScore;
            poolManager.TakeToPool(enemy.Type, enemy);
        }
    }
}