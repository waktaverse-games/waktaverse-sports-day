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
            var player = PlayFabManager.Instance.GetLeaderBoardAroundPlayer(type);
            
            for (int i = 0; i < top5RankingUI.Length; i++)
            {
                if (leaderboard.Count > i)
                {
                    var displayName = leaderboard[i].DisplayName;
                    if (displayName.Length > 5)
                    {
                        displayName = displayName.Substring(0, 4) + "...";
                    }
                    top5RankingUI[i].text = $"{displayName} : {leaderboard[i].StatValue}";
                }
                else
                {
                    top5RankingUI[i].text = "랭킹이 없어요!";
                }
            }
            
            myHighRankingUI.text = player == null ? "기록 없음" : $"{player.Position + 1}위";
        }

        public async void SetRankingBoard(MinigameType type)
        {
            var leaderboard = PlayFabManager.Instance.GetLeaderboard(type);
            
            var rankingStr = leaderboard.Count > 0 ? "" : "랭킹이 없어요!";

            foreach (var data in leaderboard)
            {
                rankingStr += $"[{data.Position + 1}위] {data.DisplayName} : {data.StatValue}점\n";
            }

            rankingList.text = rankingStr;
        }
    }
}