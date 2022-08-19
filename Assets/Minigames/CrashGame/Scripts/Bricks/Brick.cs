using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public abstract class Brick : MonoBehaviour
    {
        protected Vector2 centerPosition;
        private static int coinNetPercentage = 20;

        protected void Awake()
        {
            centerPosition = new Vector2(BrickManager.BrickWidth / 2, -BrickManager.BrickHeight / 2);
        }

        protected virtual void DropCoinByPercentage(int net)
        {
            // net 분의 1 확률로 코인 생성
            if (UnityEngine.Random.Range(0, net) == 0)
            {
                // Bronze: 90%, Silver: 9%, Gold: 1%
                int coinTypeRandom = UnityEngine.Random.Range(0, 50);
                if (coinTypeRandom < 1)
                {
                    DropCoin(GameManager.Instance.Brick.coinPrefabList[2]);
                }
                else if (coinTypeRandom < 10)
                {
                    DropCoin(GameManager.Instance.Brick.coinPrefabList[1]);
                }
                else
                {
                    DropCoin(GameManager.Instance.Brick.coinPrefabList[0]);
                }
            }
        }

        // 모든 Brick의 Parent Class
        protected virtual void DropCoin(Coin coin)
        {
            Instantiate(coin, (Vector2)transform.position + centerPosition, Quaternion.identity);
        }

        protected virtual void DestroySelf(int score)
        {
            DropCoinByPercentage(coinNetPercentage);
            GameManager.Instance.AddScore(score);
            gameObject.SetActive(false);
            GameManager.Instance.Brick.CheckOuterLineDestroyed();
        }

        public abstract void BallCollide();
    }
}
