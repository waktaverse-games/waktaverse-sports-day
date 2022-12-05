using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class ObstacleManager : MonoBehaviour
    {
        [SerializeField] GameObject VFX;
        ItemSpawner spawner;
        public void Initialize(ItemSpawner _spawner)
        {
            spawner = _spawner;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Border")
            {
                spawner.DeactiaveObstacle(gameObject);
            }
        }
    }
}
