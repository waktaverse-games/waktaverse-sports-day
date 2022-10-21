using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameHeaven.CrossGame
{
    public enum CharacterCode
    {
        Viichan,
        Gosegu,
        Jururu,
        Wakgood
    }
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

        public ObjectController ObjectController;

        [HideInInspector] public bool IsOver;

        public void AddScore(int Point)
        {
            Score += Point;
            ScoreUI.text = "Á¡¼ö : " + Score.ToString();
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
        }
    }
}

