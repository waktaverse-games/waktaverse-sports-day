using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace GameHeaven.JumpGame
{
    public class RopeManager : MonoBehaviour
    {
        public UnityEvent<bool> ReverseEvent;

        [SerializeField]
        Animator animator;
        // Game Speed
        [SerializeField] float speed;               // 줄넘기 현재 속도
        [SerializeField] float increasingSpeedTime; // 속도 증가 간격 (n초 마다)
        [SerializeField] float increasingSpeedAmount;   // 속도 증가 량  (n만큼)
        [SerializeField] float maxSpeed;            // 최대 줄넘기 속도
        [SerializeField] float minSpeed;            // 최소 줄넘기 속도

        [SerializeField] int reverseProbability;    // 리버스 확률
        [SerializeField] int slowModeProbability;   // 감속 확률
        [SerializeField] int variationStartTime;    // 리버스,변속 시작하는 시간

        private int reverseProb = 0;
        private int slowModeProb = 0;
        private bool isReverse = false;
        private bool isSlowMode = false;
        private bool isReversing = true;
        // Start is called before the first frame update
        void Start()
        {
            animator.speed = speed;
            Invoke("EnableReverse", 1f);
            StartCoroutine(IncreaseSpeed(increasingSpeedTime));
            Invoke("StartVariation", variationStartTime);
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

        public void DecreaseSpeed() // 루숙 아이템 획득상태로 충돌시 속도 반으로 줄이기
        {
            speed /= 2;
            if(speed <minSpeed) { speed = minSpeed; }
            ResetSpeedSetting();
        }

        void StartVariation()
        {
            Debug.Log("리버스, 변속 모드 시작!");
            reverseProb = reverseProbability;
            slowModeProb = slowModeProbability;
        }
        void Reverse()
        {
            if (Random.Range(0, 100) <= reverseProb && !isReversing)
            {
                Debug.Log("방향 전환!");
                isReversing = true;
                isReverse = !isReverse;
                ReverseEvent.Invoke(isReverse);
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
                Debug.Log("슬로우 모드 시작!");
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