using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using SharedLibs;
using SharedLibs.Score;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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

        [SerializeField] AnimationCounter counter;
        private int score = 0;
        public Text scoreText;
        public bool isGameOver = false;
        public bool isGameStart = false;
        private void OnEnable()
        {
            counter.OnEndCount += () =>
            {
                GameStart();
            };
        }

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
            isGameOver = true;
            ScoreManager.Instance.SetGameHighScore(MinigameType.JumpGame, score);
            ResultSceneManager.ShowResult(MinigameType.JumpGame);
        }

        void GameStart()
        {
            isGameStart = true;
            SoundManager.instance.PlayBGM();
        }
    }
}
