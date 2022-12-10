using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using SharedLibs;
using SharedLibs.Score;

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
        [SerializeField] TextMeshProUGUI textEffect;
        [SerializeField] TextMeshProUGUI scoreText;

        private Coroutine textEffectCoroutine;
        public int Score { get => score; }
        private int score = 0;
        public bool isGameOver = false;
        public bool isGameStart = false;
        private void OnEnable()
        {
            counter.OnEndCount += () =>
            {
                GameStart();
            };
        }

        public void IncreaseScore(int num)
        {
            score += num;
            scoreText.text = "점수 : " + score.ToString();
        }

        public void GameOver()
        {
            SoundManager.instance.TurnOffBGM();
            SoundManager.instance.PlayGameOverSound();
            ScoreManager.Instance.SetGameHighScore(MinigameType.BingleGame, score);
            GameResultManager.ShowResult(MinigameType.BingleGame, score);
            
            isGameOver = true;
        }

        void GameStart()
        {
            isGameStart = true;
            scoreText.text = "점수 : 0";
            SoundManager.instance.PlayBGM();
        }

        public void ShowTextEffect(string effect, bool isBlue)
        {
            if (textEffectCoroutine != null) StopCoroutine(textEffectCoroutine);
            textEffect.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Player").transform.position) + new Vector3(0, 100f, 0);
            textEffect.text = effect;
            textEffect.color = isBlue ? Color.blue : Color.red;
            textEffect.gameObject.SetActive(true);
            textEffectCoroutine = StartCoroutine(FadeText(textEffect, false));
        }

        IEnumerator FadeText(TextMeshProUGUI textUI, bool floatText, float interval = .05f)
        {
            Color textColor = textUI.color;
            textColor.a = 1f;
            textEffect.color = textColor;
            while (textColor.a > 0.05f)
            {
                textColor.a *= 0.8f;
                textUI.color = textColor;
                if (floatText) textUI.transform.Translate(0, 5f, 0);
                yield return new WaitForSeconds(interval);
            }
            textUI.gameObject.SetActive(false);
        }
    }
}
