using System;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class EnemyManager : LogicBehaviour
    {
        [SerializeField] private PlayerController controller;
        [SerializeField] private EnemyPool enemyPool;

        [SerializeField] private float upperReleaseHeight;
        [SerializeField] private float bottomReleaseHeight;
        [SerializeField] private float relocateWidth;

        private void Update()
        {
            var enemies = enemyPool.GetActiveObjects();
            for (var i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                if (enemy)
                {
                    AdjustLocation(enemy.transform);
                    ReleaseByHeight(enemy);
                }
            }
        }

        private void AdjustLocation(Transform tf)
        {
            var position = tf.position;

            var dist = position.x - controller.Position.x;
            if (dist < -relocateWidth)
                position = new Vector3(controller.Position.x + relocateWidth,
                    position.y, position.z);
            else if (dist > relocateWidth)
                position = new Vector3(controller.Position.x - relocateWidth,
                    position.y, position.z);

            tf.position = position;
        }

        private void ReleaseByHeight(Enemy enemy)
        {
            var enemyPos = enemy.transform.position;
            var playerPos = controller.Position;

            if (IsReleasable(enemyPos, playerPos)) enemyPool.TakeToPool(enemy.Type, enemy);
        }

        private bool IsReleasable(Vector3 enemyPos, Vector3 playerPos)
        {
            var upperReleaseCheck = enemyPos.y - playerPos.y > upperReleaseHeight;
            var bottomReleaseCheck = Mathf.Abs(enemyPos.x - playerPos.x) >= relocateWidth &&
                                     enemyPos.y - playerPos.y < -bottomReleaseHeight;

            return upperReleaseCheck || bottomReleaseCheck;
        }

        public override void GameReady()
        {
            var enemies = enemyPool.GetActiveObjects();
            foreach (var enemy in enemies) enemy?.Ready();
        }

        public override void GameStart()
        {
            var enemies = enemyPool.GetActiveObjects();
            foreach (var enemy in enemies) enemy?.Active();
        }

        public override void GameOver()
        {
            var enemies = enemyPool.GetActiveObjects();
            foreach (var enemy in enemies) enemy?.Inactive();
        }
    }
}