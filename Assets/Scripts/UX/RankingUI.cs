using SharedLibs;
using SharedLibs.Score;
using TMPro;
using UnityEngine;

namespace GameHeaven.UIUX
{
    public class RankingUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] top5RankingUI;
        [SerializeField] private TextMeshProUGUI myHighRankingUI;
        [SerializeField] private TextMeshProUGUI rankingList;

        public async void SetRankingUI(MinigameType type)
        {
            var leaderboard = PlayFabManager.Instance.GetLeaderboard(type);
            var highScore = ScoreManager.Instance.GetGameScore(type);
            
            for (int i = 0; i < top5RankingUI.Length; i++)
            {
                if (leaderboard.Count > i)
                {
                    top5RankingUI[i].text = $"{leaderboard[i].DisplayName} : {leaderboard[i].StatValue}";
                }
                else
                {
                    top5RankingUI[i].text = "랭킹이 없어요!";
                }
            }
            
            myHighRankingUI.text = highScore.ToString();
        }

        public async void SetRankingBoard(MinigameType type)
        {
            var leaderboard = PlayFabManager.Instance.GetLeaderboard(type);
            
            var rankingStr = leaderboard.Count > 0 ? "" : "랭킹이 없어요!";

            foreach (var data in leaderboard)
            {
                rankingStr += $"[{data.Position}] {data.DisplayName} : {data.StatValue}\n";
            }

            rankingList.text = rankingStr;
        }
    }
}