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
        [SerializeField] private GameObject rankingList;

        [SerializeField] private int showRankMax;

        public async void SetRankingUI(MinigameType type)
        {
            var leaderboard = PlayFabManager.Instance.GetLeaderboardData(type);
            var player = PlayFabManager.Instance.GetLeaderBoardAroundPlayerData(type);
            
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
            
            if (player.Position < 1) myHighRankingUI.color = Color.white;
            else if (player.Position == 1) myHighRankingUI.color = new Color(255 / 255f, 203 / 255f, 63 / 255f); // Gold
            else if (player.Position == 2) myHighRankingUI.color = new Color(232 / 255f, 232 / 255f, 232 / 255f); // Silver
            else if (player.Position == 3) myHighRankingUI.color = new Color(195 / 255f, 155 / 255f, 137 / 255f); // Bronze
            else if (player.Position <= 20) myHighRankingUI.color = new Color(77 / 255f, 231 / 255f, 78 / 255f); // Green

            myHighRankingUI.text = player is { StatValue: > 0 } ?  $"{player.Position + 1}위" : "기록 없음";
        }

        public async void SetRankingBoard(MinigameType type)
        {
            var leaderboard = PlayFabManager.Instance.GetLeaderboardData(type);
            var player = PlayFabManager.Instance.GetLeaderBoardAroundPlayerData(type);

            var boardSize = Mathf.Min(leaderboard.Count, showRankMax);
            for (var i = 0; i < boardSize; i++)
            {
                var data = leaderboard[i];
                rankingList.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = data.DisplayName;
                rankingList.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = data.StatValue.ToString();
            }
            for (var i = boardSize; i < showRankMax; i++)
            {
                rankingList.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
                rankingList.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "-";
            }

            transform.GetChild(6).GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = player.DisplayName;
            transform.GetChild(6).GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>().text = player.StatValue > 0 ? player.StatValue.ToString() : "-";
            transform.GetChild(6).GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>().text = player.StatValue > 0 ? (player.Position + 1).ToString() : "-";

            //rankingList.text = rankingStr;
        }
    }
}