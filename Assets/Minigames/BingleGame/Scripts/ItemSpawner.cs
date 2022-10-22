using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] Transform spawnPoint;
        public GameObject[] PF_items;
        public List<GameObject> itemPool;

        private void Start()
        {
            for(int i=0;i<PF_items.Length;i++)
            {
                var item = Instantiate(PF_items[i]);
                item.transform.SetParent(spawnPoint);
                item.transform.localPosition = spawnPoint.localPosition;
                item.SetActive(false);
                itemPool.Add(item);
            }
        }
        public void SpawnItem()
        {
            ResetItem();
            float xOffset = Random.Range(-3.5f, 3.5f);

            int itemIndex;
            int itemProb = Random.Range(1, 101);
            int[] probList = GameSpeedController.instance.itemProb;

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
            itemPool[itemIndex].transform.localPosition = new Vector3(xOffset, spawnPoint.localPosition.y, spawnPoint.localPosition.z);
            itemPool[itemIndex].SetActive(true);
        }

        public void ResetItem()
        {
            foreach(var item in itemPool)
            {
                item.SetActive(false);
            }
        }
    }
}
