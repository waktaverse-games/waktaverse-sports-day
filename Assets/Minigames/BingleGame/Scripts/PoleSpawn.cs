using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class PoleSpawn : MonoBehaviour
    {
        public GameObject[] checkPoints;

        public float maxSpawnDelay;

        public float maxGap;
        public float minGap;

        [SerializeField] float curTime;
        [SerializeField] bool isLeft;

        private void Update()
        {
            curTime += Time.deltaTime;

            if (curTime > maxSpawnDelay)
            {
                SpawnPoles();
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
            curTime = 0;
        }
    }
}