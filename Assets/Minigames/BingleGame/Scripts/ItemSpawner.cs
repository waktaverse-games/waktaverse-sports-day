using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class ItemSpawner : MonoBehaviour
    {

        public enum ItemType
        {
            Sushi,
            Burger,
            Bungeoppang
        }
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
            int itemIndex = Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length);
            itemPool[itemIndex].transform.localPosition = new Vector3(xOffset, spawnPoint.localPosition.y, spawnPoint.localPosition.z);
            itemPool[itemIndex].SetActive(true);
        }

        public void ResetItem()
        {
            foreach(var item in itemPool)
            {
                item.GetComponent<ItemManager>().ResetItem();
                item.SetActive(false);
            }
        }
    }
}
