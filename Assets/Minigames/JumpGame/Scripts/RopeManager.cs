using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace GameHeaven.JumpGame
{
    public class RopeManager : MonoBehaviour
    {
        public UnityEvent ExclamationMarkEvent;
        [SerializeField]
        Animator animator;

        // Game Speed
        [SerializeField] float speed;
        [SerializeField] float increasingSpeedTime;
        [SerializeField] float increasingSpeedAmount;
        [SerializeField] float maxSpeed;
        [SerializeField] float minSpeed;

        [SerializeField] int reverseProb;
        [SerializeField] int slowModeProb;

        private bool isReverse = false;
        private bool isSlowMode = false;
        private bool isReversing = true;
        // Start is called before the first frame update
        void Start()
        {
            animator.speed = speed;
            Invoke("EnableReverse", 1f);
            StartCoroutine(IncreaseSpeed(increasingSpeedTime));
        }

        IEnumerator IncreaseSpeed(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            while (!GameManager.Instance.IsGameOver)
            {
                speed += increasingSpeedAmount;
                if (speed > maxSpeed)
                    speed = maxSpeed;
                animator.speed = speed;
                yield return new WaitForSeconds(delayTime);
            }
        }

        public void DecreaseSpeed()
        {
            speed /= 2;
            if(speed <minSpeed) { speed = minSpeed; }
            ResetSpeedSetting();
        }

        void Reverse()
        {
            if (Random.Range(0, 100) <= reverseProb && !isReversing)
            {
                ExclamationMarkEvent.Invoke();
                isReversing = true;
                isReverse = !isReverse;
                if (isReverse) animator.SetFloat("reverse", -1f);
                else animator.SetFloat("reverse", 1f);
                Invoke("EnableReverse", 0.2f);
                speed *= 0.9f;
                if (speed < minSpeed) { speed = minSpeed; }
                ResetSpeedSetting();
            }
        }
        void EnableReverse()
        {
            isReversing = false;
        }

        public void SlowMode(int _isReverse)
        {
            if (Random.Range(0, 100) <= slowModeProb && isReverse == System.Convert.ToBoolean(_isReverse))
            {
                ExclamationMarkEvent.Invoke();
                float slowSpeed = speed / 2;
                if (slowSpeed < minSpeed) { slowSpeed = minSpeed; }
                StopAllCoroutines();
                animator.speed = slowSpeed;
                isSlowMode = true;
            }
        }

        public void ResetSlowMode(int _isReverse)
        {
            if(isSlowMode && isReverse == System.Convert.ToBoolean(_isReverse)) 
            {
                ResetSpeedSetting();
            }
        }
        public void JumpSuccess(AnimationEvent animEvent)
        {
            if (isReverse == System.Convert.ToBoolean(animEvent.intParameter))
            {
                GameManager.Instance.IncreaseScore((int)animEvent.floatParameter);
                GameManager.Instance.IncreaseJumpSuccessCount();
            }
        }

        public void PlayRopeSound(int _isReverse)
        {
            if (isReverse == System.Convert.ToBoolean(_isReverse))
            {
                SoundManager.Instance.PlayRopeSound();
            }
        }

        void ResetSpeedSetting()
        {
            StopAllCoroutines();
            animator.speed = speed;
            StartCoroutine(IncreaseSpeed(increasingSpeedTime));
        }
    }
}   