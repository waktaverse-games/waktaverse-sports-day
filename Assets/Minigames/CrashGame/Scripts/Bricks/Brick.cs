using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public abstract class Brick : MonoBehaviour
    {
        // ��� Brick�� Parent Class
        protected virtual void CoinDrop()
        {

        }

        protected virtual void DestroySelf(int score)
        {
            GameManager.Instance.AddScore(score);
            Destroy(gameObject);
        }

        public abstract void BallCollide();
    }
}
