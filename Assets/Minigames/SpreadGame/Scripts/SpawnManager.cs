using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject[] mobPrefabs;
        [SerializeField] GameObject[] bossPrefabs;
        [SerializeField] float normalMonsterSpawnCool, eliteMonsterSpawnCool, bossSpawnCool;
        [SerializeField] float[] mapSize;

        private void Awake()
        {
            StartCoroutine(SpawnRepeatedly(normalMonsterSpawnCool, false));
            StartCoroutine(SpawnRepeatedly(eliteMonsterSpawnCool, true));
            StartCoroutine(SpawnBossRepeatedly(bossSpawnCool));
        }

        IEnumerator SpawnRepeatedly(float sec, bool isElite) 
        {
            GameObject obj = null;

            while (true)
            {
                int idx = Random.Range(0, 100);

                if (idx < 5) idx = 0;
                else if (idx < 20) idx = 1;
                else if (idx < 35) idx = 2;
                else if (idx < 50) idx = 3;
                else if (idx < 75) idx = 4;
                else if (idx < 90) idx = 5;
                else if (idx < 100) idx = 6;

                if (isElite)
                {
                    if (idx == 0) continue;

                    yield return new WaitForSeconds(sec);
                }
                if (idx == 0) // ¹ÚÁã´Ü
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
                }
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
                    enemy.speed /= 5;
                    enemy.rigid.velocity = new Vector3(-enemy.speed, enemy.rigid.velocity.y);
                    enemy.HP += 20;
                    enemy.isElite = true;
                }

                yield return new WaitForSeconds(sec);
            }
        }

        IEnumerator SpawnBossRepeatedly(float sec)
        {
            WaitForSeconds wait = new WaitForSeconds(sec);
            GameObject obj;

            while (true)
            {
                obj = Instantiate(bossPrefabs[0], new Vector3(8, 0, 0), bossPrefabs[0].transform.rotation);

                yield return wait;
            }
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