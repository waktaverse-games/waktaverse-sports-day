using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameHeaven.CrossGame
{
    public class CrossGameManager : MonoBehaviour
    {
        int Score;
        int CollectStar;
        public Text ScoreUI;
        public Text StarUI;
        public Text GameOverTextUI;

        public ObjectController ObjectController;

        public void AddScore(int Point)
        {
            Score += Point;
            ScoreUI.text = "점수 : " + Score.ToString();
        }

        public void AddStar() 
        {
            CollectStar++;
            StarUI.text = "모은 별: " + CollectStar.ToString();
        }

        public void GameOver()
        {
            Time.timeScale = 0;
            GameOverTextUI.gameObject.SetActive(true);
        }
    }
}

