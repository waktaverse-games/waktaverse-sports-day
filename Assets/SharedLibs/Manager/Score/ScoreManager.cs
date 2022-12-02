using System;
using System.Collections.Generic;
using GameHeaven.Root;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SharedLibs.Score
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        private List<ScoreDB> _dbList;

        [SerializeField] private ScoreGoalObject goalObject;
        
        public Action<int, int> OnSetScore;
        public Action<int> OnHighScore;

        public override void Init()
        {
            _dbList = GameDatabase.Instance.DB.scoreDBList;
        }

        public void AddGameRoundScore(MinigameType type, int score)
        {
            SetGameHighScore(type, score);
        }

        public void SetGameHighScore(MinigameType type, int score)
        {
            if (GameManager.GameMode == GameMode.MinigameMode)
            {
                var rewardGoals = goalObject.GetRewardGoals(type);
                var scoreDb = _dbList.Find((data) => data.type == type);
                
                for (var i = 0; i < rewardGoals.Length; i++)
                {
                    if (score < rewardGoals[i]) continue;
                    
                    if ((scoreDb.achievement & (1 << i)) == 0)
                    {
                        scoreDb.achievement |= (1 << i);
                        PuzzleManager.GetPuzzlePiece();
                    }
                }

                OnSetScore?.Invoke(score, score);
                if (score > scoreDb.highScore)
                {
                    OnHighScore?.Invoke(score);
                    scoreDb.highScore = score;
                }
            }
        }

        public int GetGameScore(MinigameType type)
        {
            var scoreData = _dbList.Find((data) => data.type == type);
            return scoreData.highScore;
        }
        
        public int GetGameTargetScore(MinigameType type)
        {
            return goalObject.GetStoryGoal(type);
        }
    }
}