using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class CheckpointSpawner : MonoBehaviour
    {
        public GameObject[] checkPoints;

        [SerializeField] float maxSpawnDelay;

        [SerializeField] float curTime;
        [SerializeField] bool isLeft;

        private void Awake()
        {
            maxSpawnDelay = GameSpeedController.instance.cpSpawnSpeed;
        }

        private void Update()
        {
            maxSpawnDelay = GameSpeedController.instance.cpSpawnSpeed;

            curTime += Time.deltaTime;

            if (curTime > maxSpawnDelay)
            {
                SpawnPoles();
                curTime = 0;
            }
        }

        private void SpawnPoles()
        {
            int randGap = Random.Range(0, checkPoints.Length);      // 랜덤 갭 범위
            float xPos = 0;
            if (isLeft) // 좌측에 막대 생성
            {
                isLeft = false;
                switch (randGap)
                {
                    case 0:
                        xPos = Random.Range(-2f, 0);
                        break;
                    case 1:
                        xPos = Random.Range(-2f, -0.5f);
                        break;
                    case 2:
                        xPos = Random.Range(-1f, 0);
                        break;
                }
            }
            else //우측에 막대 생성
            {
                isLeft = true;
                switch (randGap)
                {
                    case 0:
                        xPos = Random.Range(0, 2f);
                        break;
                    case 1:
                        xPos = Random.Range(-0.5f, 1f);
                        break;
                    case 2:
                        xPos = Random.Range(0, 1f);
                        break;
                }
            }
            Instantiate(checkPoints[randGap], new Vector3(xPos, transform.position.y, transform.position.z), transform.rotation);
        }
    }
}