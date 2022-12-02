using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class BackgroundGenerator : MonoBehaviour
    {
        [SerializeField] Transform[] sprites;
        float camHeight;
        bool firstWarning = false;
        private void Awake()
        {
            camHeight = Camera.main.orthographicSize * 2;
        }
        public void UpdateBG(int id, float yVal)
        {
            int index = (id + 1) % 3;
            sprites[index].position = new Vector3(0, yVal - camHeight * 2, 0);
            sprites[index].gameObject.GetComponentInChildren<CheckpointSpawner>().SpawnCheckpoint();
            sprites[index].gameObject.GetComponentInChildren<Spawner>().SpawnObject();

            if (GameManager.instance.Score >= 1000)
            {
                sprites[index].gameObject.GetComponentInChildren<ObstacleSpawner>().SpawnObject();
            }
        }
    }
}
