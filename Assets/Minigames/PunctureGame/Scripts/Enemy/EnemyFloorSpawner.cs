using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    public class EnemyFloorSpawner : FloorSpawner<Enemy>
    {
        [SerializeField] [Required] private PlayerController controller;

        [SerializeField] private IntRange spawnXRange;
        [SerializeField] [ReadOnly] private float spawnYPos;

        [SerializeField] [Min(1)] private int spawnFloorInterval;
        
        [SerializeField] private float enemySpeed;
        [SerializeField] [Min(1)] private int speedIncreaseInterval;
        [SerializeField] private float speedIncreaseValue;
        [SerializeField] private float maxEnemySpeed;

        [SerializeField] private ScoreCollector scoreCollector;

        private Vector3 SpawnPosition =>
            new(controller.Position.x + Random.Range(spawnXRange.Min, spawnXRange.Max), spawnYPos);

        private int SpeedIncreaseInterval => speedIncreaseInterval * 10;

        protected override void OnEnable()
        {
            floorManager.OnFloorChanged += ChangeSpawnSpeed;
            
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            floorManager.OnFloorChanged -= ChangeSpawnSpeed;
            
            base.OnDisable();
        }

        public override void SpawnByFloor(int floor)
        {
            if (floor % spawnFloorInterval == 0 && floor >= 1) Spawn();
        }

        public void ChangeSpawnSpeed(int floor)
        {
            if (((floor <= SpeedIncreaseInterval - 10) && (floor % SpeedIncreaseInterval == 10)) ||
                (floor % SpeedIncreaseInterval == 0))
            {
                enemySpeed = Mathf.Clamp(enemySpeed + speedIncreaseValue, 0.0f, maxEnemySpeed);
            }
        }

        public override Enemy Spawn()
        {
            var enemy = GetInstance();
            spawnYPos -= spawnFloorInterval;
            if (enemy)
            {
                enemy.OnTrodden += scoreCollector.AddEnemyScore;
                
                enemy.SetPositionState(SpawnPosition);
                enemy.Controller.Speed = enemySpeed;
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