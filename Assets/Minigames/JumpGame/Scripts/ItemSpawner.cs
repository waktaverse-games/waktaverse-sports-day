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

        [SerializeField] int gomemItemFreq;
        [SerializeField] float itemSpawnDelay;

        Queue<GameObject> deactivatedItems = new Queue<GameObject>();

        void Start()
        {
            StartCoroutine(Spawn());
        }

        void SpawnItem()
        {
            GameObject item = null;
            if (GameManager.Instance.JumpSuccessCount >= gomemItemFreq && !GameManager.Instance.isInvincible)
            {

                item = Instantiate(gomemItemPrefab);
                item.transform.SetParent(transform);
                GameManager.Instance.ResetJumpSuccessCount();
            }
            else
            {
                if (GameManager.Instance.JumpSuccessCount >= gomemItemFreq) { GameManager.Instance.ResetJumpSuccessCount(); }
                item = GetItemFromPool();
                item.GetComponent<ItemManager>().InitializeItem(this, Random.Range(0, System.Enum.GetValues(typeof(Items)).Length));
            }
            float spawnPosX = Random.Range(spawnPosMin.localPosition.x, spawnPosMax.localPosition.x);
            item.transform.localPosition = new Vector3(spawnPosX, spawnPosMin.localPosition.y, 0);
        }

        public void DeactiavteItem(GameObject item)
        {
            deactivatedItems.Enqueue(item);
            item.SetActive(false);
        }
        
        GameObject GetItemFromPool()
        {
            GameObject item = deactivatedItems.Count == 0 ? Instantiate(itemPrefab) : deactivatedItems.Dequeue();
            item.SetActive(true);
            item.transform.SetParent(transform);
            return item;
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