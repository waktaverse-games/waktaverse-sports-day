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
                soundManager.BgmPlay();
            };
        }

        public void AddScore(int Point)
        {
            score += Point;
            scoreUI.text = score.ToString();
        }

        public void GameOver()
        {
            //DOTween.KillAll(true);
            //gameOverTextUI.gameObject.SetActive(true);
            //restratBtn.SetActive(true);
            soundManager.BgmStop();
            soundManager.SfxPlay("GameOver");
            objectController.player.GameOver();
            IsStop = true;
            objectController.player.cntAnimator.SetBool("GameOver", true);
            ScoreManager.Instance.SetGameHighScore(MinigameType.CrossGame, score);
            foreach (var item in objectController.flyItems)
            {
                item.GetComponent<FlyItem>().Stop();
            }
            GameResultManager.ShowResult(MinigameType.CrossGame, score);
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}

