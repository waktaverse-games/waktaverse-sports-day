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

        public void ShowEffect()
        {
            MiniGameManager.Instance.UI.ShowItemEffect($"+{coinValue}p");
        }

    }
}
