using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameHeaven.CrashGame.Define;

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

        private void Awake() => InstanceInit();

        private static void InstanceInit()
        {
            // Singleton �ʱ�ȭ
            if (instance == null)
            {
                GameObject go = GameObject.Find("GameManager");
                if (go == null)
                {
                    go = new GameObject { name = "GameManager" };
                    go.AddComponent<GameManager>();
                }
                instance = go.GetComponent<GameManager>();
            }
        }

        private GameState currentGameState;
        private int score;

        [SerializeField]
        private PlayerPlatform platform;

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

        // Update is called once per frame
        void Update()
        {

        }

        public void AddScore(int score)
        {
            Score += score;
        }

        public void GameOver()
        {
            // ���� ���� ��
            CurrentGameState = GameState.Over;
            //TODO ���� ���� UI ����.
            // �ӽ÷� ��� �����
            CurrentGameState = GameState.Start;
            platform.BallInit();
        }
    }
}

