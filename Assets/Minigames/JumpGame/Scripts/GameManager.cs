using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace GameHeaven.JumpGame
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent EnableHearthEffect;
        public UnityEvent DisableHearthEffect;
        public UnityEvent GameStartEvent;
        public UnityEvent GameEndEvent;
        public bool IsGameStart { get => isGameStart; }
        public bool IsGameOver { get => isGameOver; }
        public bool isInvincible { get; private set; }
        public int JumpSuccessCount { get => jumpSuccessCount; }

        [SerializeField] GameObject buttons;
        [SerializeField] GameObject readyButton;
        [SerializeField] GameObject startButton;
        [Space]
        [SerializeField]
        GameObject[] objects;

        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] int jumpSuccessCount = 0;

        private float totalScore = 0;
        bool isGameStart = false;
        bool isGameOver;
        #region Singleton
        public static GameManager Instance = null;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                    Destroy(this.gameObject);
            }
        }
        #endregion

        private void Start()
        {
            scoreText.text = "점수 : 0";
            StartCoroutine(StartGame());
        }
        private void Update()
        {
            Retry();
        }
        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(0.1f);
            readyButton.SetActive(true);
            yield return new WaitForSeconds(1f);
            readyButton.SetActive(false);
            startButton.SetActive(true);
            isGameStart = true;
            SoundManager.Instance.PlayBGM();
            ObjectTurnOnOff(true);
            GameStartEvent.Invoke();
            yield return new WaitForSeconds(1f);
            startButton.SetActive(false);
        }

        public void IncreaseScore(int score)
        {
            totalScore += score;
            scoreText.text = "점수 : " + totalScore.ToString();
        }
        public void IncreaseJumpSuccessCount()
        {
            jumpSuccessCount++;
        }

        public void ResetJumpSuccessCount()
        {
            jumpSuccessCount = 0;
        }

        public void SetInvinsible(bool value)
        {
            isInvincible = value;
            if (value) EnableHearthEffect.Invoke();
            else DisableHearthEffect.Invoke();
        }
        public void GameOver()
        {
            GameEndEvent.Invoke();
            ObjectTurnOnOff(false);
            isGameOver = true;
            buttons.SetActive(true);
        }

        public void Restart()
        {
            isGameOver = false;
            buttons.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        void Retry()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        void ObjectTurnOnOff(bool isTurnOn)
        {
            foreach(var obj in objects)
            {
                obj.SetActive(isTurnOn);
            }
        }
    }
}