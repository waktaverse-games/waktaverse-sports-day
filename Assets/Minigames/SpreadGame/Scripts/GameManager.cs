using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameHeaven.SpreadGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameObject[] mobPrefabs;
        [SerializeField] List<GameObject> bossPrefabs;
        public float curNormalMonsterSpawnDelay, maxNormalMonsterSpawnDelay;
        public int bossIdx = 0;
        [SerializeField] float curEliteMonsterSpawnDelay, maxEliteMonsterSpawnDelay;
        [SerializeField] float curBossSpawnDelay, maxBossSpawnDelay;
        [SerializeField] float[] mapSize;
        public bool isBossTime = false;

        [SerializeField] AudioClip bulletSound;

        private void Awake()
        {
        }

        private void Update()
        {
            SpawnRepeatedly(false);
            SpawnRepeatedly(true);
            SpawnBossRepeatedly();
        }

        void SpawnRepeatedly(bool isElite)
        {
            if (isElite)
            {
                curEliteMonsterSpawnDelay += Time.deltaTime;
                if (curEliteMonsterSpawnDelay < maxEliteMonsterSpawnDelay) return;
                curEliteMonsterSpawnDelay = 0;
            }
            else
            {
                curNormalMonsterSpawnDelay += Time.deltaTime;
                if (curNormalMonsterSpawnDelay < maxNormalMonsterSpawnDelay * (isBossTime?4:1)) return;
                curNormalMonsterSpawnDelay = 0;
            }

            GameObject obj = null;

            int idx = Random.Range(0, bossIdx + 1);

            if (isElite)
            {
                while (idx == 4)
                {
                    idx = Random.Range(0, bossIdx + 1);
                }
            }

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
            /*
            else if (idx == 6) // ¼¼±Õ
            {
                obj = Instantiate(mobPrefabs[idx], new Vector2(mapSize[0] / 2, Random.Range(-mapSize[1] / 2, mapSize[1] / 2)), mobPrefabs[idx].transform.rotation);
                StartCoroutine(Division(obj)); //ºÐ¿­
            }*/
            else obj = Instantiate(mobPrefabs[idx], new Vector2(mapSize[0] / 2, Random.Range(-mapSize[1] / 2, mapSize[1] / 2)), mobPrefabs[idx].transform.rotation);

            EnemyMove enemy = obj.GetComponent<EnemyMove>();
            enemy.HP += bossIdx / 2;
            if (enemy.HP > 10) enemy.HP = 10;

            if (isElite)
            {
                obj.transform.localScale = new Vector3(obj.transform.localScale.x * 1.5f, obj.transform.localScale.y * 1.5f, obj.transform.localScale.z);
                obj.GetComponent<SpriteRenderer>().material.color = Color.yellow;
                enemy.speed = 0.5f;
                enemy.rigid.velocity = new Vector3(-enemy.speed, enemy.rigid.velocity.y);
                enemy.HP *= 3;
                enemy.isElite = true;
            }
        }

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
            
            obj = Instantiate(bossPrefabs[bossIdx % 7], Vector2.zero, bossPrefabs[0].transform.rotation);
            obj.transform.GetChild(0).GetComponent<BossMove>().HP = obj.transform.GetChild(0).GetComponent<BossMove>().maxHP = 100 + bossIdx * 100;
            isBossTime = true;
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