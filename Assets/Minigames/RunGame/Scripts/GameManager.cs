using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{ 
    public class GameManager : MonoBehaviour
    {
        public GameObject wall;
        [SerializeField] GameObject[] coin;
        public Transform spawnPoints;
        public GameObject dust;
        public Transform dustPoints;

        public float wallSpawnDelay;
        public float curSpawnDelay;

        public float coinSpawnDelay;
        public float curCoinSpawnDelay;

        public static float gameTime;
        public static float wallSpeed;
        public float timer;

        // Update is called once per frame
        void Start()
        {
            gameTime = 0;
            wallSpeed = 2;
        }

        void Update()
        {
            curSpawnDelay += Time.deltaTime;
            curCoinSpawnDelay += Time.deltaTime;
            gameTime += Time.deltaTime;
            timer += Time.deltaTime;

            if (curSpawnDelay > wallSpawnDelay)
            {
                SpawnWall();
                SpawnDust();
                curSpawnDelay = 0;
                wallSpawnDelay = 2 / wallSpeed;
            }

            if (curCoinSpawnDelay > coinSpawnDelay)
            {
                SpawnCoin();
                curCoinSpawnDelay = 0;
                coinSpawnDelay = Random.Range(4, 7);
            }
        }

        void SpawnWall()
        {
            Instantiate(wall, spawnPoints.position, spawnPoints.rotation);
        }

        void SpawnCoin()
        {
            Instantiate(coin[Random.Range(0, coin.Length)], spawnPoints.position + new Vector3(Random.Range(-3f, 3f),0, 0), spawnPoints.rotation);
        }

        void SpawnDust()
        {
            Instantiate(dust, dustPoints.position + new Vector3(Random.Range(-0.5f, 0.5f), -1, 0), dustPoints.rotation);
        }
    }
}