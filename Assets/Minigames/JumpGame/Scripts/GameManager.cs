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

        [SerializeField] AnimationCounter counter;
        [SerializeField] GameObject[] objects;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] int jumpSuccessCount = 0;

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
            scoreText.text = "점수 : 0";
            isGameStart = true;
            SoundManager.Instance.PlayBGM();
            ObjectTurnOnOff(true);
            GameStartEvent.Invoke();
        }

        public void IncreaseScore(int score)
        {
            totalScore += score;
            scoreText.text = "점수 : " + totalScore.ToString();
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
            ScoreManager.Instance.SetGameHighScore(MinigameType.JumpGame, totalScore);
            ResultSceneManager.ShowResult(MinigameType.JumpGame);
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