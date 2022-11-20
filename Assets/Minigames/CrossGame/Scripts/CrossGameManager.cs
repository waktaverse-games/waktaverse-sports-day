using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        public GameObject RestratBtn;
        public ObjectController ObjectController;
        public SoundManager SoundManager;

        [HideInInspector] public bool IsOver;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

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
            //DOTween.KillAll(true);
            GameOverTextUI.gameObject.SetActive(true);
            RestratBtn.SetActive(true);
            IsOver = true;
            ObjectController.Player.CntAnimator.SetBool("GameOver", true);
            RestratBtn.SetActive(true);
            //ScoreManager.Instance.AddGameRoundScore(MinigameType.CrossGame, Score);
            foreach (var item in ObjectController.FlyItems)
            {
                item.GetComponent<FlyItem>().Stop();
            }

        }

        void Balanceing()
        {
            if (ObjectController.MovementSpeed < 7.5f)
            {
                ObjectController.MovementSpeed += 0.04f;
            }
            else if(ObjectController.MovementSpeed < 9)
            {
                ObjectController.MovementSpeed += 0.005f;
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}

