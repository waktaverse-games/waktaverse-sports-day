using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using SharedLibs;
using SharedLibs.Score;

namespace GameHeaven.JumpGame
{
    public class GameManager : MonoBehaviour
    {
        public UnityEvent EnableHearthEffect;
        public UnityEvent DisableHearthEffect;
        public UnityEvent GameStartEvent;
        public UnityEvent GameEndEvent;
        public bool IsGameStart { get => isGameStart; }
        public bool IsGameOver { get => isGameOver; }
        public bool isInvincible { get; private set; }
        public int JumpSuccessCount { get => jumpSuccessCount; }
        public int Score { get => totalScore; }
        [SerializeField] AnimationCounter counter;
        [SerializeField] GameObject[] objects;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI textEffect;
        [SerializeField] int jumpSuccessCount = 0;

        private Coroutine textEffectCoroutine;
        private int totalScore = 0;
        bool isGameStart = false;
        bool isGameOver;
        #region Singleton
        public static GameManager Instance = null;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                    Destroy(this.gameObject);
            }
        }
        #endregion

        private void OnEnable()
        {
            counter.OnEndCount += () =>
            {
                GameStart();
            };
        }

        void GameStart()
        {
            scoreText.text = "���� : 0";
            isGameStart = true;
            SoundManager.Instance.PlayBGM();
            ObjectTurnOnOff(true);
            GameStartEvent.Invoke();
        }

        public void IncreaseScore(int score)
        {
            totalScore += score;
            scoreText.text = "���� : " + totalScore.ToString();
        }
        public void IncreaseJumpSuccessCount()
        {
            jumpSuccessCount++;
        }

        public void ResetJumpSuccessCount()
        {
            jumpSuccessCount = 0;
        }

        public void SetInvinsible(bool value)
        {
            isInvincible = value;
            if (value) EnableHearthEffect.Invoke();
            else DisableHearthEffect.Invoke();
        }
        public void GameOver()
        {
            GameEndEvent.Invoke();
            ObjectTurnOnOff(false);
            isGameOver = true;
            SoundManager.Instance.TurnOffBGM();
            SoundManager.Instance.PlayGameOverSound();
            ScoreManager.Instance.SetGameHighScore(MinigameType.JumpGame, totalScore);
            GameResultManager.ShowResult(MinigameType.JumpGame, totalScore);
        }
        public void ShowTextEffect(string effect)
        {
            if (textEffectCoroutine != null) StopCoroutine(textEffectCoroutine);
            textEffect.transform.position = Camera.main.WorldToScreenPoint(GameObject.Find("Player").transform.position) + new Vector3(0,100f,0);
            textEffect.text = effect;
            textEffect.gameObject.SetActive(true);
            textEffectCoroutine = StartCoroutine(FadeText(textEffect, true));
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

        void ObjectTurnOnOff(bool isTurnOn)
        {
            foreach(var obj in objects)
            {
                obj.SetActive(isTurnOn);
            }
        }


    }
}