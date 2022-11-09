using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace GameHeaven.JumpGame
{
    public class GameManager : MonoBehaviour
    {
        public bool IsGameOver { get;private set; }
        public bool isInvincible { get; set; }
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
        public void MainMenu()
        {
            print("Go to main menu");
        }
    }
}