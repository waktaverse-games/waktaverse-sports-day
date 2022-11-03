using UnityEngine;

namespace SharedLibs.Score
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [System.Serializable]
        private class ScoreDictionary : UnitySerializedDictionary<MinigameType, int> {}
        
        [SerializeField]
        private ScoreDictionary scoreDic = new ScoreDictionary() {};
        
        public int AllScore { get; private set; }

        public override void Init() {
            
        }

        public void AddGameRoundScore(MinigameType type, int score)
        {
            AllScore += score;
            scoreDic[type] += score;
        }

        public void SetGameHighScore(MinigameType type, int score)
        {
            scoreDic[type] = score;
        }

        public int GetGameScore(MinigameType type)
        {
            return scoreDic[type];
        }
    }
}