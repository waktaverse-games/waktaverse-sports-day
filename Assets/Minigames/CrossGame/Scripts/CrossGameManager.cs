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
        int score;
        int collectStar;
        public TMP_Text scoreUI;
        public TMP_Text gameOverTextUI;
        public GameObject restratBtn;
        public ObjectController objectController;
        public CrossGameSoundManager soundManager;
        public AnimationCounter counter;

        [HideInInspector] public bool IsStop = true;

        private void OnEnable()
        {
            counter.OnEndCount += () =>
            {
                IsStop = false;
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public void AddScore(int Point)
        {
            score += Point;
            scoreUI.text = "Á¡¼ö : " + score.ToString();
        }

        public void GameOver()
        {
            //DOTween.KillAll(true);
            gameOverTextUI.gameObject.SetActive(true);
            restratBtn.SetActive(true);
            IsStop = true;
            objectController.player.cntAnimator.SetBool("GameOver", true);
            restratBtn.SetActive(true);
            //ScoreManager.Instance.AddGameRoundScore(MinigameType.CrossGame, Score);
            foreach (var item in objectController.flyItems)
            {
                item.GetComponent<FlyItem>().Stop();
            }

        }



        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}

