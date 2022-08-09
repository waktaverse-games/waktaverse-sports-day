using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class Brick : MonoBehaviour
    {
        [SerializeField]
        protected Sprite brickSprite;
        protected static int scoreAdd = 5;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void DestroySelf()
        {
            GameManager.Instance.AddScore(scoreAdd);
            Destroy(gameObject);
        }

        public virtual void BallCollide()
        {
            DestroySelf();
        }
    }
}
