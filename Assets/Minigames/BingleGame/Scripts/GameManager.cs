using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using SharedLibs;

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
        public bool isGameStart = false;

        public GameObject retryButton;

        [SerializeField] GameObject readyButton;
        [SerializeField] GameObject startButton;

        void Start()
        {
            StartCoroutine(StartGame());
        }
        void Update()
        {
            scoreText.text = string.Format("{0:n0}", score);
            ReStart();
        }
        public void IncreaseScore(int num)
        {
            score += num;
        }

        public void GameOver()
        {
            isGameOver = true;
            startButton.SetActive(false);   //혹시 게임 시작하고 바로 죽는경우를 대비해서...
            //SharedLibs.Score.ScoreManager.Instance.AddGameRoundScore(MinigameType.BingleGame, score);
            retryButton.SetActive(true);
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        IEnumerator StartGame()
        {
            yield return new WaitForSeconds(0.1f);
            readyButton.SetActive(true);
            yield return new WaitForSeconds(1f);
            readyButton.SetActive(false);
            startButton.SetActive(true);
            isGameStart = true;
            yield return new WaitForSeconds(1f);
            startButton.SetActive(false);
        }
        void ReStart()
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
