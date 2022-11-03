using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameObject[] mobPrefabs;
        [SerializeField] List<GameObject> bossPrefabs;
        public float curNormalMonsterSpawnDelay, maxNormalMonsterSpawnDelay;
        [SerializeField] float curEliteMonsterSpawnDelay, maxEliteMonsterSpawnDelay;
        [SerializeField] float curBossSpawnDelay, maxBossSpawnDelay;
        [SerializeField] float[] mapSize;
        public bool isBossTime = false;

        [SerializeField] AudioClip bulletSound;

        private void Awake()
        {
            Screen.SetResolution(960, 540, false);
        }

        private void Update()
        {
            SpawnRepeatedly(false);
            SpawnRepeatedly(true);
            SpawnBossRepeatedly();
        }

        void SpawnRepeatedly(bool isElite)
        {
            if (isBossTime)
            {
                curEliteMonsterSpawnDelay = curNormalMonsterSpawnDelay = 0;
                return;
            }

            if (isElite)
            {
                curEliteMonsterSpawnDelay += Time.deltaTime;
                if (curEliteMonsterSpawnDelay < maxEliteMonsterSpawnDelay) return;
                curEliteMonsterSpawnDelay = 0;
            }
            else
            {
                curNormalMonsterSpawnDelay += Time.deltaTime;
                if (curNormalMonsterSpawnDelay < maxNormalMonsterSpawnDelay) return;
                curNormalMonsterSpawnDelay = 0;
            }

            GameObject obj = null;

            int idx = Random.Range(0, bossIdx + 1);
            /*
            while (true)
            {
                if (idx < ) idx = 0;
                else if (idx < 20) idx = 1;
                else if (idx < 35) idx = 2;
                else if (idx < 50) idx = 3;
                else if (idx < 75) idx = 4;
                else if (idx < 90) idx = 5;
                else if (idx < 100) idx = 6;

                if (!isElite || idx != 0) break;
            }*/

            if (idx == 4) // ¹ÚÁã´Ü
            {
                Vector2 spawnPos = new Vector2(mapSize[0] / 2, Random.Range(-mapSize[1] / 2, mapSize[1] / 2));
                Instantiate(mobPrefabs[idx], spawnPos + new Vector2(-1, 0.5f), mobPrefabs[idx].transform.rotation);
                Instantiate(mobPrefabs[idx], spawnPos + new Vector2(-1, -0.5f), mobPrefabs[idx].transform.rotation);
                Instantiate(mobPrefabs[idx], spawnPos + new Vector2(0, 1), mobPrefabs[idx].transform.rotation);
                Instantiate(mobPrefabs[idx], spawnPos, mobPrefabs[idx].transform.rotation);
                Instantiate(mobPrefabs[idx], spawnPos + new Vector2(0, -1), mobPrefabs[idx].transform.rotation);
                Instantiate(mobPrefabs[idx], spawnPos + new Vector2(1, 1), mobPrefabs[idx].transform.rotation);
                Instantiate(mobPrefabs[idx], spawnPos + new Vector2(1, 0), mobPrefabs[idx].transform.rotation);
                Instantiate(mobPrefabs[idx], spawnPos + new Vector2(1, -1), mobPrefabs[idx].transform.rotation);
                Instantiate(mobPrefabs[idx], spawnPos + new Vector2(2, 0.5f), mobPrefabs[idx].transform.rotation);
                Instantiate(mobPrefabs[idx], spawnPos + new Vector2(2, -0.5f), mobPrefabs[idx].transform.rotation);
            } // ¹ÚÁã
            else if (idx == 6) // ¼¼±Õ
            {
                obj = Instantiate(mobPrefabs[idx], new Vector2(mapSize[0] / 2, Random.Range(-mapSize[1] / 2, mapSize[1] / 2)), mobPrefabs[idx].transform.rotation);
                StartCoroutine(Division(obj)); //ºÐ¿­
            }
            else obj = Instantiate(mobPrefabs[idx], new Vector2(mapSize[0] / 2, Random.Range(-mapSize[1] / 2, mapSize[1] / 2)), mobPrefabs[idx].transform.rotation);

            if (isElite)
            {
                EnemyMove enemy = obj.GetComponent<EnemyMove>();
                obj.transform.localScale = new Vector3(obj.transform.localScale.x * 1.5f, obj.transform.localScale.y * 1.5f, obj.transform.localScale.z);
                obj.GetComponent<SpriteRenderer>().material.color = Color.yellow;
                enemy.speed = 0.5f;
                enemy.rigid.velocity = new Vector3(-enemy.speed, enemy.rigid.velocity.y);
                enemy.HP += 20;
                enemy.isElite = true;
            }
        }

        [SerializeField] int bossIdx = 0;
        void SpawnBossRepeatedly()
        {
            if (isBossTime)
            {
                curBossSpawnDelay = 0;
                return;
            }

            curBossSpawnDelay += Time.deltaTime;
            if (curBossSpawnDelay < maxBossSpawnDelay) return;
            curBossSpawnDelay = 0;

            if (isBossTime) return;

            GameObject obj;
            
            obj = Instantiate(bossPrefabs[bossIdx++ % 4], Vector2.zero, bossPrefabs[0].transform.rotation);
            isBossTime = true;
            maxNormalMonsterSpawnDelay -= 0.3f;
            if (maxNormalMonsterSpawnDelay < 2) maxNormalMonsterSpawnDelay = 2;
        }

        IEnumerator Division(GameObject obj)
        {
            WaitForSeconds wait = new WaitForSeconds(3.0f);
            yield return wait;

            while (obj != null)
            {
                obj.GetComponent<SpriteRenderer>().material.color = Color.green;
                obj.GetComponent<Rigidbody2D>().velocity *= 0.8f;
                obj.GetComponent<EnemyMove>().speed *= 0.8f;
                obj = Instantiate(mobPrefabs[6], obj.transform.position, mobPrefabs[6].transform.rotation);
                yield return wait;
            }
        }
    }
}