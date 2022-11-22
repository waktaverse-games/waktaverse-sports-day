using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class EnemyCollision : MonoBehaviour
    {
        public GameManager gameManager;
        public GameObject starEffect;

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
                starEffect.SetActive(true);
                starEffect.GetComponent<Animator>().Play("effect");
                Invoke("DisableEffect", 0.3f);
            }
            if (col.CompareTag("UpgradeItem"))
            {
                col.gameObject.SetActive(false);
                gameManager.ItemActivate();
            }
        }

        private void DisableEffect()
        {
            starEffect.SetActive(false);
        }
    }
}