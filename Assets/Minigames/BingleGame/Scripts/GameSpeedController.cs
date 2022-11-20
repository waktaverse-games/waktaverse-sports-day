using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class GameSpeedController : MonoBehaviour
    {
        #region Singleton
        public static GameSpeedController instance = null;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                    Destroy(this.gameObject);
            }
        }
        #endregion

        // Game Speed
        public float xSpeed;
        public float ySpeed;
        public float xMaxSpeed;
        public float yMaxSpeed;
        public float increasingXSpeedTime;
        public float increasingXSpeedAmount;
        public float increasingYSpeedTime;
        public float increasingYSpeedAmount;
        private void Start()
        {
            StartCoroutine(IncreaseXSpeed(increasingXSpeedTime));
            StartCoroutine(IncreaseYSpeed(increasingYSpeedTime));
        }
        IEnumerator IncreaseXSpeed(float delayTime)
        {
            while(!GameManager.instance.isGameOver)
            {
                xSpeed += increasingXSpeedAmount;
                if (xSpeed >= xMaxSpeed) 
                    xSpeed = xMaxSpeed;
                yield return new WaitForSeconds(delayTime);
            }
        }
        IEnumerator IncreaseYSpeed(float delayTime)
        {
            while (!GameManager.instance.isGameOver)
            {
                if (ySpeed >= yMaxSpeed) 
                    ySpeed = yMaxSpeed;
                ySpeed += increasingYSpeedAmount;
                yield return new WaitForSeconds(delayTime);
            }
        }
    }
}
