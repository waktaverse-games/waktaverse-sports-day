using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class BackgroundCollision : MonoBehaviour
    {
        [SerializeField] int id;

        [SerializeField] CheckpointSpawner checkpointSpawner;
        [SerializeField] BackgroundGenerator backgroundGenerator;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                backgroundGenerator.UpdateBG(id, transform.position.y);
            }
        }
    }
}