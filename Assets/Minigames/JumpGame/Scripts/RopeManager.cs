using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class RopeManager : MonoBehaviour
    {
        [SerializeField]
        Animator animator;

        // Game Speed
        public float speed;
        public float increasingSpeedTime;
        public float increasingSpeedAmount;
        public float maxSpeed;

        private int timeCount = 0;
        // Item Spawn Probability
        public int[] itemProb;

        // Start is called before the first frame update
        void Start()
        {
            animator.speed = speed;
            StartCoroutine(IncreaseSpeed(increasingSpeedTime));
            itemProb = new int[3] { 90, 9, 1 };
        }

        IEnumerator IncreaseSpeed(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            speed += increasingSpeedAmount;
            if (speed > maxSpeed)
                speed = maxSpeed;
            animator.speed = speed;

            timeCount++;
            AdjustItemProb();

            StartCoroutine(IncreaseSpeed(increasingSpeedTime));
        }
        private void AdjustItemProb()
        {
            switch (timeCount)
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

        public void JumpSuccess()
        {
            GameManager.Instance.IncreaseScore();
        }
    }
}   