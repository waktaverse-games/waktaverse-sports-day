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
        [SerializeField] float speed;               // �ٳѱ� ���� �ӵ�
        [SerializeField] float increasingSpeedTime; // �ӵ� ���� ���� (n�� ����)
        [SerializeField] float increasingSpeedAmount;   // �ӵ� ���� ��  (n��ŭ)
        [SerializeField] float maxSpeed;            // �ִ� �ٳѱ� �ӵ�
        [SerializeField] float minSpeed;            // �ּ� �ٳѱ� �ӵ�

        [SerializeField] int reverseProbability;    // ������ Ȯ��
        [SerializeField] int slowModeProbability;   // ���� Ȯ��
        [SerializeField] int variationStartTime;    // ������,���� �����ϴ� �ð�

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

        public void DecreaseSpeed() // ��� ������ ȹ����·� �浹�� �ӵ� ������ ���̱�
        {
            speed /= 2;
            if(speed <minSpeed) { speed = minSpeed; }
            ResetSpeedSetting();
        }

        void StartVariation()
        {
            Debug.Log("������, ���� ��� ����!");
            reverseProb = reverseProbability;
            slowModeProb = slowModeProbability;
        }
        void Reverse()
        {
            if (Random.Range(0, 100) <= reverseProb && !isReversing)
            {
                Debug.Log("���� ��ȯ!");
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
                Debug.Log("���ο� ��� ����!");
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