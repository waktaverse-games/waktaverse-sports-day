using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class DeactivateOnBorder : MonoBehaviour
    {
        PoolManager pool;


        private void Awake()
        {
            pool = FindObjectOfType<PoolManager>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Border"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
