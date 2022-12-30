using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class CoinDropBrick : Brick
    {
        private static int coinNetPercentage = 10;
        private ItemManager itemManager;

        protected override void Awake()
        {
            base.Awake();
            itemManager = MiniGameManager.Instance.Item;
        }

        protected virtual void DropCoinByPercentage(int net)
        {
            // net 분의 1 확률로 코인 생성
            if (UnityEngine.Random.Range(0, net) == 0)
            {
                DropCoin(MiniGameManager.Instance.Brick.coinPrefab);
            }
        }

        // 모든 Brick의 Parent Class
        protected virtual void DropCoin(Coin coin)
        {
            Coin coinObject = MiniGameManager.ObjectPool.GetObject("coin").GetComponent<Coin>();
            coinObject.transform.position = transform.position;
            coinObject.transform.SetParent(itemManager.ItemParent, true);
        }

        protected override void DestroySelf(int score)
        {
            DropCoinByPercentage(coinNetPercentage);
            base.DestroySelf(score);
        }

        public override void BallCollide()
        {

        }
    }
}

