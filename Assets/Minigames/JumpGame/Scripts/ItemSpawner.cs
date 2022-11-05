using SharedLibs.Score;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class ItemSpawner : MonoBehaviour
    {
        enum Items
        {
            bronzeCoin,
            silverCoin,
            GoldCoin
        }
        [SerializeField] Transform spawnPosMin;
        [SerializeField] Transform spawnPosMax;
        [SerializeField] GameObject itemPrefab;
        [SerializeField] GameObject gomemItemPrefab;

        [SerializeField] float itemSpawnDelay;

        Queue<GameObject> deactivatedItems = new Queue<GameObject>();

        void Start()
        {
            StartCoroutine(Spawn());
        }

        void SpawnItem()
        {
            GameObject item = GetItemFromPool();
            float spawnPosX = Random.Range(spawnPosMin.localPosition.x, spawnPosMax.localPosition.x);
            item.SetActive(true);
            item.transform.SetParent(transform);
            item.transform.localPosition = new Vector3(spawnPosX, spawnPosMin.localPosition.y, 0);
            item.GetComponent<Animator>().SetInteger("itemNum", Random.Range(0, System.Enum.GetValues(typeof(Items)).Length));
            item.GetComponent<ItemPoolConnector>().Spawner = this;
        }

        public void DeactiavteItem(GameObject item)
        {
            deactivatedItems.Enqueue(item);
        }

        
        GameObject GetItemFromPool()
        {
            return deactivatedItems.Count == 0 ? Instantiate(itemPrefab) : deactivatedItems.Dequeue();
        }
        
        IEnumerator Spawn()
        {
            yield return new WaitForSeconds(itemSpawnDelay);    // 게임시작후 일정시간 뒤에 코루틴 시작

            while (!GameManager.Instance.IsGameOver)
            {
                SpawnItem();
                yield return new WaitForSeconds(itemSpawnDelay);
            }

        }
    }
}