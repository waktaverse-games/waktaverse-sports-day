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
    public enum ScoreType
    {
        item,
        pass,
        flag
    }

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
        [SerializeField] int itemScore;
        [SerializeField] int passScore;
        [SerializeField] int flagScore;

        private Coroutine textEffectCoroutine;
        int totalScore = 0;
        int comboScore = 0;
        bool isGameOver = false;
        bool isGameStart = false;

        public int TotalScore { get => totalScore; }
        public bool IsGameOver { get => isGameOver; }
        public bool IsGameStart { get => isGameStart; }
        public int ComboScore { get=>comboScore; }
        private void OnEnable()
        {
            counter.OnEndCount += () =>
            {
                GameStart();
            };
        }

        public void IncreaseScore(ScoreType type)
        {
            totalScore += ScoreMap(type);
            scoreText.text = totalScore.ToString();
        }

        public void GameOver()
        {
            SoundManager.instance.TurnOffBGM();
            SoundManager.instance.PlayGameOverSound();
            ScoreManager.Instance.SetGameHighScore(MinigameType.BingleGame, totalScore);
            GameResultManager.ShowResult(MinigameType.BingleGame, totalScore);
            
            isGameOver = true;
        }

        void GameStart()
        {
            isGameStart = true;
            scoreText.text = "0";
            SoundManager.instance.PlayBGM();
        }

        public void ShowTextEffect()
        {
            if (textEffectCoroutine != null) StopCoroutine(textEffectCoroutine);
            textEffect.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Player").transform.position) + new Vector3(0, 100f, 0);
            textEffect.text = comboScore == 1 ? $"ÄÞº¸ + {comboScore}" : $"+ {comboScore}"; ;
            textEffect.color = Color.red;
            textEffect.gameObject.SetActive(true);
            textEffectCoroutine = StartCoroutine(FadeText(textEffect, false));
        }

        public void ComboReset() { comboScore = 0; }
        public void IncreaseCombo() { comboScore++; }
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

        int ScoreMap(ScoreType type)
        {
            if (type == ScoreType.item) return itemScore;
            if (type == ScoreType.pass) return passScore;
            if (type == ScoreType.flag) return flagScore + comboScore;
            return 0;
        }

    }
}
