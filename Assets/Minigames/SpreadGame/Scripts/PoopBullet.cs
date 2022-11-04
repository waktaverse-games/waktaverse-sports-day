using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class PoopBullet : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            print("Collision");
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            print("trigger");
            if (collision.CompareTag("Border"))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}