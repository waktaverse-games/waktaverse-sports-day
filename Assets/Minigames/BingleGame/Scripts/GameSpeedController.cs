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

        // Item Spawn Probability
        public int[] itemProb;

        private int timeCount = 0;
        private void Start()
        {
            StartCoroutine(IncreaseSpeed(increasingSpeedTime));
            itemProb = new int[3] { 90, 9, 1 };
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
            timeCount++;
            AdjustItemProb();

            StartCoroutine(IncreaseSpeed(increasingSpeedTime));
        }
        private void AdjustItemProb()
        {
            switch(timeCount)
            {
                case 5:
                    itemProb = new int[3] { 85, 13, 2 };
                    break;
                case 10:
                    itemProb = new int[3] { 80, 17, 3 };
                    break;
                case 15:
                    itemProb = new int[3] { 75, 21, 4 };
                    break;
                case 20:
                    itemProb = new int[3] { 70, 25, 5 };
                    break;
                case 25:
                    itemProb = new int[3] { 65, 28, 7 };
                    break;
                case 30:
                    itemProb = new int[3] { 60, 30, 10 };
                    break;
            }
        }
    }
}
