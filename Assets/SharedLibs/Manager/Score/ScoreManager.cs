using UnityEngine;

namespace SharedLibs.Score
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [System.Serializable]
        private class ScoreDictionary : UnitySerializedDictionary<MinigameType, int> {}
        
        [SerializeField]
        private ScoreDictionary scoreDic;
        
        public int AllScore { get; private set; }

        private void Awake()
        {
            scoreDic = new ScoreDictionary();
        }

        public void AddGameRoundScore(MinigameType type, int score)
        {
            AllScore += score;
            scoreDic[type] += score;
        }

        public int GetGameScore(MinigameType type)
        {
            return scoreDic[type];
        }
    }
}