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

        public static event Action<MinigameType, int> OnHighScoreChanged; 

        public override void Init()
        {
            _dbList = GameDatabase.Instance.DB.scoreDBList;
        }

        public async void SetGameHighScore(MinigameType type, int score)
        {
            if (GameManager.GameMode != GameMode.MinigameMode) return;
            
            var scoreDb = _dbList.Find((data) => data.type == type);
            var entry = PlayFabManager.Instance.GetLeaderBoardAroundPlayerData(type);
            
            if (score > entry.StatValue)
            {
                entry.StatValue = score;
                OnHighScoreChanged?.Invoke(type, score);
            }
        }
        
        public int SetGameAchievement(MinigameType type, int score)
        {
            var scoreDb = _dbList.Find((data) => data.type == type);
            var rewardGoals = goalObject.GetRewardGoals(type);
                
            for (var i = 0; i < rewardGoals.Length; i++)
            {
                if (score < rewardGoals[i])
                {
                    return i;
                }
                    
                if ((scoreDb.achievement & (1 << i)) == 0)
                {
                    scoreDb.achievement |= (1 << i);
                    GameDatabase.Instance.DB.puzzleDB.pieceCount++;
                }
            }

            return rewardGoals.Length;
        }
        
        public int GetGameAchievement(MinigameType type)
        {
            var scoreData = _dbList.Find((data) => data.type == type);

            var i = 0;
            while ((scoreData.achievement & (1 << i)) != 0)
            {
                i++;
            }

            return i;
        }
        
        public int GetGameTargetScore(MinigameType type)
        {
            return goalObject.GetStoryGoal(type);
        }
    }
}