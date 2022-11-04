using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class DeactivateOnBorder : MonoBehaviour
    {
        PoolManager pool;
        [SerializeField] bool isPoop;


        private void Awake()
        {
            pool = FindObjectOfType<PoolManager>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isPoop)
            {
                if (collision.CompareTag("Border") || collision.gameObject.layer == 13)
                {
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    gameObject.layer = 13;
                }
            }
            else if (collision.CompareTag("Border"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
