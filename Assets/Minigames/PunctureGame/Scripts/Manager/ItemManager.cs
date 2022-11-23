using System;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class ItemManager : LogicBehaviour
    {
        [SerializeField] private PlayerController controller;
        [SerializeField] private ItemPool itemPool;

        [SerializeField] private float upperReleaseHeight;
        [SerializeField] private float relocateWidth;

        private void Awake()
        {
            controller = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            var items = itemPool.GetActiveObjects();
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (item && item.enabled)
                {
                    ItemXAxisRelocate(item.transform);
                    ItemReleaseConditionCheck(item);
                }
            }
        }

        private void ItemXAxisRelocate(Transform tf)
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

        private void ItemReleaseConditionCheck(Item item)
        {
            var enemyPos = item.transform.position;
            var playerPos = controller.Position;

            if (IsItemReleasable(enemyPos, playerPos)) itemPool.TakeToPool(item.Type, item);
        }

        private bool IsItemReleasable(Vector3 itemPos, Vector3 playerPos)
        {
            return itemPos.y - playerPos.y > upperReleaseHeight;
        }

        public override void GameReady()
        {
            var items = itemPool.GetActiveObjects();
            foreach (var item in items) item?.Ready();
        }

        public override void GameStart()
        {
            var items = itemPool.GetActiveObjects();
            foreach (var item in items) item?.Active();
        }

        public override void GameOver()
        {
            var items = itemPool.GetActiveObjects();
            foreach (var item in items) item?.Inactive();
        }
    }
}