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

            int itemIndex;
            int itemProb = Random.Range(1, 101);
            int[] probList = GameSpeedController.instance.itemProb;
            Debug.Log($"Item Prob = {itemProb}");
            if(itemProb <= probList[2])
            {
                itemIndex = 2;
            }
            else if(itemProb <= probList[1])
            {
                itemIndex = 1;
            }
            else
            {
                itemIndex = 0;
            }
            Instantiate(items[itemIndex], spawnPoint, transform.rotation);
        }
    }
}
