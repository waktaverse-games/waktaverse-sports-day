using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SharedLibs;
using SharedLibs.Score;
using SharedLibs.Character;

namespace GameHeaven.CrossGame
{
    public enum CoinCode
    {
        Bronze,
        Silver,
        Gold
    }
    public class CrossGameManager : MonoBehaviour
    {
        int Score;
        int CollectStar;
        public Text ScoreUI;
        public Text StarUI;
        public Text GameOverTextUI;
        public int a => Score == 3 ? 1 : 2;

        public ObjectController ObjectController;

        [HideInInspector] public bool IsOver;

        public void AddScore(int Point)
        {
            Score += Point;
            ScoreUI.text = "Á¡¼ö : " + Score.ToString();
            if (Point == 10)
            {
                Balanceing();
            }
            else if (Point == 20)
            {
                Balanceing();
                Balanceing();
            }
        }

        public void AddGold(int num)
        {
            CollectStar += num;
            StarUI.text = "°ñµå: " + CollectStar.ToString();
        }

        public void GameOver()
        {
            GameOverTextUI.gameObject.SetActive(true);
            IsOver = true;
            ObjectController.Player.CntAnimator.SetBool("GameOver", true);
            ScoreManager.Instance.AddGameRoundScore(MinigameType.CrossGame, Score);
        }

        void Balanceing()
        {
            ObjectController.MovementSpeed *= 1.01f;
            ObjectController.JumpSpeed += 0.001f;
        }
    }
}

