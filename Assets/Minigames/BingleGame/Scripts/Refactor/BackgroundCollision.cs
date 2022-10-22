using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class BackgroundCollision : MonoBehaviour
    {
        [SerializeField] int id;

        [SerializeField] BackgroundGenerator backgroundGenerator;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                backgroundGenerator.MoveBG(id, transform.position.y);
            }
        }
    }
}