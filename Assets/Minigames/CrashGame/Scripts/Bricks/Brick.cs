using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public abstract class Brick : MonoBehaviour
    {
        [Obsolete]
        protected Vector2 centerPosition;

        protected virtual void Awake()
        {
            centerPosition = new Vector2(BrickManager.BrickWidth / 2, -BrickManager.BrickHeight / 2);
        }

        protected virtual void DestroySelf(int score)
        {
            
            GameManager.Instance.AddScore(score);
            gameObject.SetActive(false);
            GameManager.Instance.Brick.CheckOuterLineDestroyed(false);
        }

        public abstract void BallCollide();
    }
}
