using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
namespace GameHeaven.BingleGame
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        public static GameManager instance = null;
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

        private int score = 0;
        public Text scoreText;
        public bool isGameOver = false;

        public GameObject retryButton;
        void Update()
        {
            scoreText.text = string.Format("{0:n0}", score);
            if(isGameOver)
            {
                GameOver();
            }
        }
        public void IncreaseScore(int num)
        {
            score += num;
        }

        public void GameOver()
        {
            Time.timeScale = 0;
            retryButton.SetActive(true);
        }

        public void Retry()
        {
            Time.timeScale = 1;
            isGameOver = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
