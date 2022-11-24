using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class ScoreCollector : LogicBehaviour
    {
        [SerializeField] private ItemScore itemScore;
        [SerializeField] private int enemyScore;
        [SerializeField] private int floorScore;

        [SerializeField] [ReadOnly] private int totalScore;

        [SerializeField] private FloorManager floorManager;
        // [SerializeField] private EnemyFloorSpawner enemySpawner;

        [SerializeField] [ReadOnly] private bool isCollectable;

        public event Action<int, int> OnAddScore;

        public int TotalScore => totalScore;

        private void Awake()
        {
            totalScore = 0;
        }

        private void OnEnable()
        {
            floorManager.OnFloorChanged += AddFloorScore;
        }

        private void OnDisable()
        {
            floorManager.OnFloorChanged -= AddFloorScore;
        }

        private void AddScore(int score)
        {
            totalScore += score;
            OnAddScore?.Invoke(score, totalScore);
        }

        public void AddFloorScore(int floor)
        {
            if (!isCollectable) return;
            
            AddScore(floorScore);
            Debug.Log($"Floor: {floorScore} ({totalScore})");
        }

        public void AddEnemyScore()
        {
            if (!isCollectable) return;

            AddScore(enemyScore);
            Debug.Log($"Enemy: {enemyScore} ({totalScore})");
        }

        public void AddItemScore(string type)
        {
            if (!isCollectable) return;

            var score = itemScore.GetItemScoreByType(type);
            AddScore(score);
            Debug.Log($"Item: {score} ({totalScore})");
        }

        public override void GameReady()
        {
            isCollectable = false;
        }

        public override void GameStart()
        {
            isCollectable = true;
        }

        public override void GameOver()
        {
            isCollectable = false;
        }
    }
}