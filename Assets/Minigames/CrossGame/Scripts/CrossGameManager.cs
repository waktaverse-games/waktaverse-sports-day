using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
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
        public TMP_Text ScoreUI;
        public TMP_Text GameOverTextUI;

        public ObjectController ObjectController;
        public SoundManager SoundManager;

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
        }

        public void GameOver()
        {
            DOTween.KillAll(true);
            GameOverTextUI.gameObject.SetActive(true);
            IsOver = true;
            ObjectController.Player.CntAnimator.SetBool("GameOver", true);
            //ScoreManager.Instance.AddGameRoundScore(MinigameType.CrossGame, Score);
            
/*            foreach (GameObject tmp in ObjectController.FlyItems)
            {
                DOTween.Kill(tmp.transform);
            }*/
            
        }

        void Balanceing()
        {
            if (ObjectController.MovementSpeed < 7.5f)
            {
                ObjectController.MovementSpeed += 0.05f;
            }
            else if(ObjectController.MovementSpeed < 9)
            {
                ObjectController.MovementSpeed += 0.005f;
            }
        }
    }
}

