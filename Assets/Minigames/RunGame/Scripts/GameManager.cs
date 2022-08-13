using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{ 
    public class GameManager : MonoBehaviour
    {
        public GameObject wall;
        public Transform spawnPoints;

        public float maxSpawnDelay;
        public float curSpawnDelay;

        // Update is called once per frame
        void Update()
        {
            curSpawnDelay += Time.deltaTime;

            if (curSpawnDelay > maxSpawnDelay)
            {
                SpawnWall();
                curSpawnDelay = 0;
            }
        }

        void SpawnWall()
        {
            Instantiate(wall, spawnPoints.position, spawnPoints.rotation);
        }
    }
}