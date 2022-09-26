using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class EnemyCollision : MonoBehaviour
    {
        public GameManager gameManager;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                gameManager.GameOver();
            }

            if (col.CompareTag("Coin"))
            {
                col.gameObject.SetActive(false);
                gameManager.AddCoin();
            }
        }
    }
}