using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace GameHeaven.JumpGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] GameObject buttons;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] float jumpScore;
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
        public void IncreaseScore()
        {
            totalScore += jumpScore;
            scoreText.text = "점수 : " + totalScore.ToString();
        }

        public void GameOver()
        {
            buttons.SetActive(true);
            Time.timeScale = 0;
        }

        public void Restart()
        {
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