using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class Coin : MonoBehaviour
    {
        [SerializeField]
        private int coinValue;

        private Rigidbody2D rigidBody;

        public int CoinValue
        {
            get { return coinValue; }
        }

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            GetComponent<SpriteRenderer>().sprite = MiniGameManager.Instance.Brick.foodSpriteList[Random.Range(0, 3)];
        }

        public void ShowEffect()
        {
            MiniGameManager.Instance.UI.ShowItemEffect($"+{coinValue}p");
        }

    }
}
