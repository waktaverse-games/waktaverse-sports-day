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
            Bungeoppang,
            Sushi,
            Burger
        }
        [SerializeField] Transform spawnPosMin;
        [SerializeField] Transform spawnPosMax;
        [SerializeField] GameObject itemPrefab;
        [SerializeField] GameObject gomemItemPrefab;
        [SerializeField] GameObject obstaclePrefab;

        [SerializeField] int gomemItemFreq;
        [SerializeField] float itemSpawnDelay;

        [SerializeField] float obstacleSpawnDelayMin;
        [SerializeField] float obstacleSpawnDelayMax;
        Queue<GameObject> deactivatedItems = new Queue<GameObject>();
        Queue<GameObject> deactivatedObstacles = new Queue<GameObject>();

        bool gotGomemItem = false;
        void Start()
        {
            StartCoroutine(Spawn());
            StartCoroutine(SpawnObstacle());
        }

        void SpawnItem()
        {
            GameObject item = null;
            if (GameManager.Instance.JumpSuccessCount >= gomemItemFreq && !GameManager.Instance.isInvincible && !gotGomemItem)
            {
                item = Instantiate(gomemItemPrefab);
                item.transform.SetParent(transform);
                GameManager.Instance.ResetJumpSuccessCount();
                gotGomemItem = true;
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

        void SpawnObstacles()
        {
            GameObject obstacle = Instantiate(obstaclePrefab, transform);
            float spawnPosX = Random.Range(spawnPosMin.localPosition.x, spawnPosMax.localPosition.x);
            obstacle.transform.localPosition = new Vector3(spawnPosX, spawnPosMin.localPosition.y, 0);
            obstacle.GetComponent<ObstacleManager>().Initialize(this);
        }

        public void DeactiavteItem(GameObject item)
        {
            deactivatedItems.Enqueue(item);
            item.SetActive(false);
        }
        public void DeactiaveObstacle(GameObject obstacle)
        {
            deactivatedObstacles.Enqueue(obstacle);
            obstacle.SetActive(false);
        }
        GameObject GetItemFromPool()
        {
            GameObject item = deactivatedItems.Count == 0 ? Instantiate(itemPrefab) : deactivatedItems.Dequeue();
            item.SetActive(true);
            item.transform.SetParent(transform);
            return item;
        }
        
        GameObject GetObstacleFromPool()
        {
            GameObject obstacle = deactivatedObstacles.Count == 0 ? Instantiate(obstaclePrefab) : deactivatedObstacles.Dequeue();
            obstacle.SetActive(true);
            obstacle.transform.SetParent(transform);
            return obstacle;
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
        IEnumerator SpawnObstacle()
        {
            yield return new WaitForSeconds(Random.Range(obstacleSpawnDelayMin, obstacleSpawnDelayMax));
            while (!GameManager.Instance.IsGameOver)
            {
                SpawnObstacles();
                if(GameManager.Instance.Score >= 1500)
                {
                    SpawnObstacle();
                }
                yield return new WaitForSeconds(Random.Range(obstacleSpawnDelayMin, obstacleSpawnDelayMax));
            }
        }

        void UpdateObstacleSpawnSpeed()
        {
            if(GameManager.Instance.Score >= 2000)
            {
                obstacleSpawnDelayMin = 3f;
                obstacleSpawnDelayMax = 5f;
            }
            else if(GameManager.Instance.Score >= 1000)
            {
                obstacleSpawnDelayMin = 3f;
                obstacleSpawnDelayMax = 8f;
            }
        }
    }
}