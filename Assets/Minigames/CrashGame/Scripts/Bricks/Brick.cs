using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public abstract class Brick : MonoBehaviour
    {
        protected Vector2 centerPosition;

        protected void Awake()
        {
            centerPosition = new Vector2(BrickManager.BrickWidth / 2, -BrickManager.BrickHeight / 2);
        }

        // ¸ðµç BrickÀÇ Parent Class
        protected virtual void DropCoin()
        {

        }

        protected virtual void DestroySelf(int score)
        {
            GameManager.Instance.AddScore(score);
            gameObject.SetActive(false);
            GameManager.Instance.Brick.CheckOuterLineDestroyed();
        }

        public abstract void BallCollide();
    }
}
