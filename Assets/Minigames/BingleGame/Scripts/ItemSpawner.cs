using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] float maxSpawnDelay;
        [SerializeField] float curTime;

        public Transform spawnPointLeft;
        public Transform spawnPointRight;

        public GameObject[] items;
        private void Awake()
        {
            maxSpawnDelay = GameSpeedController.instance.cpSpawnSpeed * 2f;
        }

        void Update()
        {
            maxSpawnDelay = GameSpeedController.instance.cpSpawnSpeed * 2f;

            curTime += Time.deltaTime;

            if (curTime > maxSpawnDelay)
            {
                SpawnItem();
                curTime = 0;
            }

        }

        private void SpawnItem()
        {
            float xCord = Random.Range(spawnPointLeft.position.x, spawnPointRight.position.x);
            Vector3 spawnPoint = new Vector3(xCord, transform.position.y, transform.position.z);

            int itemIndex = Random.Range(0, items.Length);

            Instantiate(items[itemIndex], spawnPoint, transform.rotation);
        }
    }
}
