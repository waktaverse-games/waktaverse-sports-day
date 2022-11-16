using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    public class EnemyManager : LogicAffectManager
    {
        [SerializeField] private Player player;
        [SerializeField] private EnemyPoolSpawner spawner;
        
        [SerializeField] private float upperReleaseHeight;
        [SerializeField] private float bottomReleaseHeight;
        [SerializeField] private float relocateWidth;

        private void OnDisable()
        {
            Inactive();
        }

        private void Update()
        {
            var enemies = spawner.GetActiveElements();
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                if (enemy && enemy.enabled)
                {
                    EnemyXAxisRelocate(enemy.transform);
                    EnemyReleaseConditionCheck(enemy);
                }
            }
        }

        private void EnemyXAxisRelocate(Transform tf)
        {
            var position = tf.position;

            var dist = position.x - player.currentPos.x;
            if (dist < -relocateWidth)
                position = new Vector3(player.currentPos.x + relocateWidth,
                    position.y, position.z);
            else if (dist > relocateWidth)
                position = new Vector3(player.currentPos.x - relocateWidth,
                    position.y, position.z);

            tf.position = position;
        }

        private void EnemyReleaseConditionCheck(Enemy enemy)
        {
            var enemyPos = enemy.transform.position;
            var playerPos = player.currentPos;

            if (IsEnemyReleasable(enemyPos, playerPos))
            {
                spawner.Release(enemy);
            }
        }

        private bool IsEnemyReleasable(Vector3 enemyPos, Vector3 playerPos)
        {
            var upperReleaseCheck = enemyPos.y - playerPos.y > upperReleaseHeight;
            var bottomReleaseCheck = Mathf.Abs(enemyPos.x - playerPos.x) >= relocateWidth && (enemyPos.y - playerPos.y < -bottomReleaseHeight);
            
            return upperReleaseCheck || bottomReleaseCheck;
        }
        
        // Abstract Implement

        public override void Active()
        {
            throw new NotImplementedException();
        }

        public override void Inactive()
        {
            var enemies = spawner.GetActiveElements();
            foreach (LogicBehaviour enemy in enemies)
            {
                enemy.Inactive();
            }
        }
    }
}