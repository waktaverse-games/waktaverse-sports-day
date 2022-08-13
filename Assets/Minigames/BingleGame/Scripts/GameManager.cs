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

        void Update()
        {
            scoreText.text = string.Format("{0:n0}", score);
        }
        public void IncreaseScore(int num)
        {
            score += num;
        }

        public void GameOver()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
