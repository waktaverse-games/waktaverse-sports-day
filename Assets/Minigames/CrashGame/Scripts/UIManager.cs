using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameHeaven.CrashGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Text gameScore;
        [SerializeField]
        private Text highScore;
        [SerializeField]
        private Text coin;

        [SerializeField]
        private GameObject GameOverUI;

        public void SetScoreText(int score)
        {
            gameScore.text = $"����: {score}";
        }

        public void SetHighScoreText(int score)
        {
            highScore.text = $"�ְ���: {score}";
        }

        public void SetCoinText(int coin)
        {
            this.coin.text = $"����: {coin}";
        }

        public void GameOver()
        {
            GameOverUI.SetActive(true);
        }

        public void RestartGame()
        {
            GameManager.Instance.GameStart();
            GameOverUI.SetActive(false);
        }
    }
}

