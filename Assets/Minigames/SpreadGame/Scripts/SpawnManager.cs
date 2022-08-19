using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject[] mobPrefabs;
        [SerializeField] float SpawnCool;
        [SerializeField] float[] mapSize;

        private void Awake()
        {
            StartCoroutine(SpawnRepeatedly(SpawnCool));
        }

        IEnumerator SpawnRepeatedly(float sec)
        {
            while (true)
            {
                int idx = Random.Range(0, 100);

                if (idx < 5) idx = 0;
                else if (idx < 15) idx = 4;
                else if (idx < 30) idx = 2;
                else if (idx < 45) idx = 4;
                else if (idx < 60) idx = 4;
                else if (idx < 80) idx = 5;
                else if (idx < 100) idx = 6;

                while (idx == 1 || idx == 3) idx = Random.Range(0, mobPrefabs.Length); // ÀÓ½Ã

                if (idx == 0) // ¹ÚÁã´Ü
                {
                    Vector2 spawnPos = new Vector2(mapSize[0] / 2 + 1, Random.Range(-mapSize[1] / 2, mapSize[1] / 2));
                    Instantiate(mobPrefabs[idx], spawnPos + new Vector2(-1, 0.5f), mobPrefabs[idx].transform.rotation);
                    Instantiate(mobPrefabs[idx], spawnPos + new Vector2(-1, -0.5f), mobPrefabs[idx].transform.rotation);
                    Instantiate(mobPrefabs[idx], spawnPos + new Vector2(0, 1), mobPrefabs[idx].transform.rotation);
                    Instantiate(mobPrefabs[idx], spawnPos, mobPrefabs[idx].transform.rotation);
                    Instantiate(mobPrefabs[idx], spawnPos + new Vector2(0, -1), mobPrefabs[idx].transform.rotation);
                    Instantiate(mobPrefabs[idx], spawnPos + new Vector2(-1, 1), mobPrefabs[idx].transform.rotation);
                    Instantiate(mobPrefabs[idx], spawnPos + new Vector2(-1, 0), mobPrefabs[idx].transform.rotation);
                    Instantiate(mobPrefabs[idx], spawnPos + new Vector2(-1, -1), mobPrefabs[idx].transform.rotation);
                    Instantiate(mobPrefabs[idx], spawnPos + new Vector2(2, 0.5f), mobPrefabs[idx].transform.rotation);
                    Instantiate(mobPrefabs[idx], spawnPos + new Vector2(2, -0.5f), mobPrefabs[idx].transform.rotation);
                }
                else if (idx == 6) // ¼¼±Õ
                {
                    StartCoroutine(Division(Instantiate(mobPrefabs[idx], new Vector2(mapSize[0] / 2 + 1, Random.Range(-mapSize[1] / 2, mapSize[1] / 2)), mobPrefabs[idx].transform.rotation))); //ºÐ¿­
                }
                else Instantiate(mobPrefabs[idx], new Vector2(mapSize[0] / 2 + 1, Random.Range(-mapSize[1] / 2, mapSize[1] / 2)), mobPrefabs[idx].transform.rotation);

                yield return new WaitForSeconds(sec);
            }
        }

        IEnumerator Division(GameObject obj)
        {
            WaitForSeconds wait = new WaitForSeconds(3.0f);
            yield return wait;

            while (obj != null)
            {
                obj.GetComponent<SpriteRenderer>().material.color = Color.green;
                obj.GetComponent<Rigidbody2D>().velocity += Vector2.left * 2f;
                obj.GetComponent<EnemyMove>().speed += 2f;
                obj = Instantiate(mobPrefabs[6], obj.transform.position, mobPrefabs[6].transform.rotation);
                yield return wait;
            }
        }
    }
}