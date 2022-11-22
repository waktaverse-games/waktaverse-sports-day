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
        public bool IsGameOver { get;private set; }
        public bool isInvincible { get; private set; }
        public int JumpSuccessCount { get => jumpSuccessCount; }
        [SerializeField] GameObject buttons;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] int jumpSuccessCount = 0;

        private float totalScore = 0;
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
        }
        private void Update()
        {
            Retry();
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
            IsGameOver = true;
            buttons.SetActive(true);
            Time.timeScale = 0;
        }

        public void Restart()
        {
            IsGameOver = false;
            buttons.SetActive(false);
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        void Retry()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        public void MainMenu()
        {
            print("Go to main menu");
        }
    }
}