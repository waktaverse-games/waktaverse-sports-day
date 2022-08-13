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

        public float speed;
        public float increasingSpeedTime;
        public float increasingSpeedAmount;
        private void Start()
        {
            StartCoroutine(IncreaseSpeed(increasingSpeedTime));

        }
        IEnumerator IncreaseSpeed(float delayTime)
        {
            Debug.Log("CoroutineCalled");
            yield return new WaitForSeconds(delayTime);
            speed += increasingSpeedAmount;
            StartCoroutine(IncreaseSpeed(increasingSpeedTime));
        }

    }
}
