using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharedLibs.Score
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [System.Serializable]
        public class ScoreDicValue
        {
            public MinigameType type;
            public int score;
        }

        [SerializeField] private List<ScoreDicValue> scoreList;
        private Dictionary<MinigameType, int> scoreDic;

        /// <summary>
        /// (added score, total score, origin score)
        /// </summary>
        public Action<int, int> OnAddScore;
        /// <summary>
        /// (new score, prev score)
        /// </summary>
        public Action<int, int> OnSetScore;

        public int AllScore { get; private set; }

        public override void Init()
        {
            scoreDic = new Dictionary<MinigameType, int>();
            foreach (var data in scoreList)
            {
                scoreDic.Add(data.type, data.score);
            }
        }

        public void AddGameRoundScore(MinigameType type, int score)
        {
            OnAddScore?.Invoke(score, scoreDic[type]);
            
            AllScore += score;
            scoreDic[type] += score;
#if UNITY_EDITOR
            scoreList.Find((val) => val.type == type).score += score;
#endif
        }

        public void SetGameHighScore(MinigameType type, int score)
        {
            OnAddScore?.Invoke(score, scoreDic[type]);
            
            scoreDic[type] = score;
#if UNITY_EDITOR
            scoreList.Find((val) => val.type == type).score = score;
#endif
        }

        public int GetGameScore(MinigameType type)
        {
            return scoreDic[type];
        }
    }
}