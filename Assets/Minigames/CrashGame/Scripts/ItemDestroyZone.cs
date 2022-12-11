using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class ItemDestroyZone : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Coin") || collision.collider.CompareTag("CrashGame_Item"))
            {
                Destroy(collision.gameObject);
            }
        }
    }

}
