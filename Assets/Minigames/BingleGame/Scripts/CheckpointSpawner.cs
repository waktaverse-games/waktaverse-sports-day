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
            int randGap = Random.Range(0, checkPoints.Length);      // ���� �� ����
            float xPos = 0;
            if (isLeft) // ������ ���� ����
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
            else //������ ���� ����
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