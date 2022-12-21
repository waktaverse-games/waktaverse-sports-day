using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class CheckpointSpawner : MonoBehaviour
    {
        [SerializeField] GameObject[] PF_checkpointTypes;
        [SerializeField] Transform[] spawnPoints;

        Dictionary<int, List<GameObject>> checkpointPool = new Dictionary<int, List<GameObject>>();

        private int numCheckpointsPerBG = 2;

        private void Start()
        {
            for (int i = 0; i < numCheckpointsPerBG; i++)
            {
                checkpointPool.Add(i, new List<GameObject>());
                for (int j = 0; j < PF_checkpointTypes.Length; j++)
                {
                    var cp = Instantiate(PF_checkpointTypes[j], spawnPoints[i]);
                    cp.transform.SetParent(spawnPoints[i]);
                    checkpointPool[i].Add(cp);
                    cp.SetActive(false);
                }
            }
        }

        public void SpawnCheckpoint()
        {
            DisableCheckpoint();
            for (int i = 0; i < numCheckpointsPerBG; i++)
            {
                int cpType = Random.Range(0, spawnPoints.Length);
                float xOffset = 0;
                switch(cpType)
                {
                    case 0:
                        xOffset = Random.Range(1f, 2f);
                        break;
                    case 1:
                        xOffset = Random.Range(1f, 1.5f);
                        break;
                    case 2:
                        xOffset = Random.Range(1f, 1.2f);
                        break;
                }

                xOffset = (i % 2 == 0) ? xOffset : xOffset * -1f;
                float yOffset = Random.Range(-0.5f, 0.5f);
                var cp = checkpointPool[i][cpType];    // n개 타입중에 하나 랜덤 생성
                cp.transform.localPosition = new Vector3(xOffset, yOffset, 0);
                cp.SetActive(true);
                cp.GetComponent<CheckPointManager>().ResetCheckpoint();
            }
        }

        public void DisableCheckpoint()
        {
            for(int i=0;i<numCheckpointsPerBG;i++)
            {
                for(int j=0;j<PF_checkpointTypes.Length;j++)
                {
                    checkpointPool[i][j].SetActive(false);
                }
            }
        }
    }
}