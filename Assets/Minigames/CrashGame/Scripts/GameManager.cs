using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameHeaven.CrashGame.Utils;

namespace GameHeaven.CrashGame
{
    public class GameManager : MonoBehaviour
    {
        // singleton pattern
        private static GameManager instance = null;
        public static GameManager Instance
        {
            get
            {
                InstanceInit();
                return instance;
            } 
        }

        private UIManager uiManager;
        private BrickManager brickManager;
        private GameState currentGameState;
        private int score;
        private int highscore;
        private int money;

        public int Money
        {
            get { return money; }
            set { money = value; }
        }

        public Ball ballPrefab;
        //public GameObject testBallPrefab;

        public PlayerPlatform platform;

        public UIManager UI
        {
            get 
            {
                if (uiManager == null) uiManager = gameObject.AddComponent<UIManager>();
                return uiManager; 
            }
        }

        public BrickManager Brick
        {
            get
            {
                if (brickManager == null) brickManager = gameObject.AddComponent<BrickManager>();
                return brickManager;
            }
        }

        public GameState CurrentGameState
        {
            get { return currentGameState; }
            private set { currentGameState = value; }
        }

        public int Score
        {
            get { return score; }
            private set { score = value; }
        }

        private void Awake()
        {
            InstanceInit();
            uiManager = GetComponent<UIManager>();
            brickManager = GetComponent<BrickManager>();
            score = 0;
            highscore = 0;
            uiManager.SetScoreText(Score);
            uiManager.SetHighScoreText(Score);
        }

        private void Start()
        {
            GameStart();
        }

        private static void InstanceInit()
        {
            // Singleton 초기화
            if (instance == null)
            {
                GameObject go = GameObject.Find("GameManager");
                if (go == null)
                {
                    go = new GameObject { name = "GameManager" };
                    go.AddComponent<GameManager>();
                    go.AddComponent<BrickManager>();
                    go.AddComponent<UIManager>();
                }
                instance = go.GetComponent<GameManager>();
            }
        }



        // Update is called once per frame
        void Update()
        {
            uiManager.SetScoreText(Score);
        }

        public void AddScore(int score)
        {
            Score += score;
        }

        public void GameOver()
        {
            // 게임 오버 시
            //TODO 게임 오버 UI 띄우기.
            CurrentGameState = GameState.Over;
            if (Score > highscore)
            {
                highscore = Score;
                uiManager.SetHighScoreText(Score);
            }
            // 임시로 즉시 재시작
            GameStart();
        }

        public void GameStart()
        {
            Ball.BallNumber = 0;
            Score = 0;
            CurrentGameState = GameState.Start;
            brickManager.ResetBricks();
            platform.BallInit();
        }
    }
}

