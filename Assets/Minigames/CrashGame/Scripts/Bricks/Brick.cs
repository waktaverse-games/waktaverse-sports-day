using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public abstract class Brick : MonoBehaviour, IDestroyEffect
    {
        [Obsolete]
        protected Vector2 centerPosition;
        private GameObject brickDestroyEffect;

        protected virtual void Awake()
        {
            //centerPosition = new Vector2(BrickManager.BrickWidth / 2, -BrickManager.BrickHeight / 2);
            try
            {
                brickDestroyEffect = transform.GetChild(0).gameObject;
                brickDestroyEffect.SetActive(false);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                Debug.LogError("No block destroy effect");
            }
        }

        protected virtual void DestroySelf(int score)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            brickDestroyEffect.SetActive(true);
            GameManager.Instance.AddScore(score);
        }

        public void DestroyAfterEffect()
        {
            brickDestroyEffect.SetActive(false);
            gameObject.SetActive(false);
            GameManager.Instance.Brick.CheckOuterLineDestroyed(false);
        }

        public abstract void BallCollide();
    }
}
