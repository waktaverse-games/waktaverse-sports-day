using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    public class ItemFloorSpawner : FloorSpawner<Item>
    {
        [SerializeField] [Required] private PlayerController controller;

        [SerializeField] private IntRange spawnXRange;
        [SerializeField] private IntRange spawnYInterval;
        [SerializeField] private float spawnTriggerDist;
        [SerializeField] [ReadOnly] private float spawnYPos;

        [SerializeField] private PunctureSFXCollection sfxCollection;
        [SerializeField] private ScoreCollector scoreCollector;

        private Vector3 SpawnPosition =>
            new(controller.Position.x + Random.Range(spawnXRange.Min, spawnXRange.Max), spawnYPos);

        private int NextYInterval => Random.Range(spawnYInterval.Min, spawnYInterval.Max + 1);

        public override void SpawnByFloor(int floor)
        {
            if (controller.Position.y - spawnTriggerDist <= spawnYPos) Spawn();
        }

        public override Item Spawn()
        {
            var item = GetInstance();

            spawnYPos -= NextYInterval;
            if (item)
            {
                item.transform.position = SpawnPosition;

                item.OnGetting += ItemGetEvent;
                item.OnRelease += Release;
            }

            return item;
        }

        public override void Release(Item item)
        {
            item.OnGetting -= ItemGetEvent;
            item.OnRelease -= Release;
            
            poolManager.TakeToPool(item.Type, item);
        }

        private void ItemGetEvent(string name)
        {
            sfxCollection.PlaySFX(PunctureSFXType.ItemGet);
            scoreCollector.AddItemScore(name);
        }
    }
}