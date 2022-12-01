using System.Collections.Generic;
using SharedLibs;
using UnityEngine;

namespace GameHeaven.Root
{
    [System.Serializable]
    public class ScoreGoalData
    {
        [SerializeField] private MinigameType type;
        [SerializeField] private int storyGoal;
        [SerializeField] private int[] rewardGoals;

        public MinigameType Type => type;
        public int StoryGoal => storyGoal;
        public int[] RewardGoals => rewardGoals;
    }
    
    [CreateAssetMenu(fileName = "Score Goals Data", menuName = "Minigame/Score Goals Data", order = 0)]
    public class ScoreGoalObject : ScriptableObject
    {
        [SerializeField] private List<ScoreGoalData> goalDataList;

        public int GetStoryGoal(MinigameType type)
        {
            return goalDataList.Find((data) => data.Type == type).StoryGoal;
        }
        
        public int[] GetRewardGoals(MinigameType type)
        {
            return goalDataList.Find((data) => data.Type == type).RewardGoals;
        }
    }
}