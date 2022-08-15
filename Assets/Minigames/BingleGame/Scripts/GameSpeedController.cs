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
        public float speed;
        public float increasingSpeedTime;
        public float increasingSpeedAmount;

        // Checkpoint Spawn Speed
        public float cpSpawnSpeed;
        public float cpSpawnSpeedAmount;
        public float cpSpawnSpeedMin;
        private void Start()
        {
            StartCoroutine(IncreaseSpeed(increasingSpeedTime));

        }
        IEnumerator IncreaseSpeed(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            speed += increasingSpeedAmount;

            if (cpSpawnSpeed > cpSpawnSpeedMin)
            {
                cpSpawnSpeed -= cpSpawnSpeedAmount;
            }
            else
            {
                cpSpawnSpeed = cpSpawnSpeedMin;
            }
            StartCoroutine(IncreaseSpeed(increasingSpeedTime));
        }

    }
}
