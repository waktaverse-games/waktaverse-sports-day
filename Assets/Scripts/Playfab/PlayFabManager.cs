using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using SharedLibs;
using SharedLibs.Score;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PlayFabLeaderboardElement
{
    [SerializeField] private MinigameType type;
    [SerializeField] [ReadOnly] private List<PlayerLeaderboardEntry> entries;
    [SerializeField] [ReadOnly] private PlayerLeaderboardEntry playerEntry;

    public MinigameType Type => type;
    public List<PlayerLeaderboardEntry> Entries
    {
        get => entries;
        internal set => entries = value;
    }
    public PlayerLeaderboardEntry PlayerEntry
    {
        get => playerEntry;
        internal set => playerEntry = value;
    }
}

public class PlayFabManager : MonoSingleton<PlayFabManager>
{
    [SerializeField] private List<PlayFabLeaderboardElement> leaderboardEntries;

    [SerializeField] [ReadOnly] private string playFabId;
    
    [SerializeField] [ReadOnly] private int lastUpdateMinute;

    // private Coroutine routine;

    [Header("[DEBUG] 활성화 시 테스트 리더보드로 연결")] [SerializeField]
    private bool isDebug;

    public override void Init()
    {
        Login();

        lastUpdateMinute = -1;
    }

    private void OnEnable()
    {
        playFabId = "";
        ScoreManager.OnHighScoreChanged += SendLeaderboard;

        // routine = StartCoroutine(LeaderBoardUpdateRoutine(delayMinute));
    }
    
    private void OnDisable()
    {
        playFabId = "";
        ScoreManager.OnHighScoreChanged -= SendLeaderboard;
        
        // StopCoroutine(routine);
    }

    private void Login()
    {
        var req = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true, InfoRequestParameters =
                new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true
                }
        };

        PlayFabClientAPI.LoginWithCustomID(req, result =>
        {
            Debug.Log("Login Success");
            playFabId = result.PlayFabId;

            var profile = result.InfoResultPayload.PlayerProfile;
            if (profile != null)
                GameDatabase.NickName = profile.DisplayName;
            else
                UpdateDisplayName(GameDatabase.NickName);

            UpdateLeaderBoard();
        }, error => Debug.LogError("Login Failed (" + error.Error.ToString() + "): " + error.ErrorMessage));
    }

    public void SendLeaderboard(MinigameType type, int score)
    {
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new()
                {
                    StatisticName = GetStasticName(type),
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(req, result => Debug.Log("Update Success"),
            error => Debug.LogError("Update Failed (" + error.Error.ToString() + "): " + error.ErrorMessage));
    }

    public List<PlayerLeaderboardEntry> GetLeaderboard(MinigameType type)
    {
        var element = leaderboardEntries.Find(t => t.Type == type);
        return element.Entries;
    }

    public PlayerLeaderboardEntry GetLeaderBoardAroundPlayer(MinigameType type)
    {
        var element = leaderboardEntries.Find(t => t.Type == type);
        return element.PlayerEntry;
    }

    // private IEnumerator LeaderBoardUpdateRoutine(int delay)
    // {
    //     while (true)
    //     {
    //         if (!isLoggedIn)
    //         {
    //             yield return null;
    //             continue;
    //         }
    //
    //         UpdateLeaderBoard();
    //
    //         yield return new WaitForSeconds(delay * 60);
    //     }
    // }

    public void UpdateLeaderBoard()
    {
        if (lastUpdateMinute == DateTime.Now.Minute)
            return;
        lastUpdateMinute = DateTime.Now.Minute;
        
        Debug.Log("Update Leaderboard");
        
        foreach (var element in leaderboardEntries)
        {
            var leaderboardReq = new GetLeaderboardRequest
            {
                StatisticName = GetStasticName(element.Type),
                StartPosition = 0,
                MaxResultsCount = 10
            };
            var playerReq = new GetLeaderboardAroundPlayerRequest
            {
                StatisticName = GetStasticName(element.Type),
                MaxResultsCount = 1
            };
            StartCoroutine(UpdateLeaderBoardElement(element, leaderboardReq, playerReq));
        }
    }

    private IEnumerator UpdateLeaderBoardElement(PlayFabLeaderboardElement element, GetLeaderboardRequest leaderboardReq, GetLeaderboardAroundPlayerRequest playerReq)
    {
        var isLeaderboardComplete = false;
        var isPlayerComplete = false;
        PlayFabClientAPI.GetLeaderboard(leaderboardReq, result =>
        {
            element.Entries = result.Leaderboard;
            isLeaderboardComplete = true;
            Debug.Log("Get Leaderboard Success");
        }, error =>
        {
            Debug.LogError("Get Leaderboard Failed (" + error.Error.ToString() + "): " + error.ErrorMessage);
            isLeaderboardComplete = true;
            throw new Exception(error.ErrorMessage);
        });
        PlayFabClientAPI.GetLeaderboardAroundPlayer(playerReq, result =>
        {
            var entry = result.Leaderboard[0];
            if (entry.PlayFabId == playFabId)
            {
                if (entry.Position != 1 || entry.StatValue != 0)
                {
                    element.PlayerEntry = entry;
                    Debug.Log(entry.Position + " : " + entry.StatValue);
                }
                else
                {
                    element.PlayerEntry = null;
                    Debug.Log(element.Type.ToString() + " - 없음");
                }
            }
            isPlayerComplete = true;
            Debug.Log("Get LeaderboardAroundPlayer Success");
        }, error =>
        {
            Debug.LogError("Get LeaderboardAroundPlayer Failed (" + error.Error.ToString() + "): " + error.ErrorMessage);
            isPlayerComplete = true;
            throw new Exception(error.ErrorMessage);
        });

        yield return new WaitUntil((() => isLeaderboardComplete && isPlayerComplete));
    }

    public void UpdateDisplayName(string displayName)
    {
        GameDatabase.NickName = displayName;

        var req = new UpdateUserTitleDisplayNameRequest { DisplayName = displayName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(req, result => { Debug.Log("UpdateUserTitleDisplayName Success"); },
            error => { Debug.LogError("UpdateUserTitleDisplayName Error (" + error.Error.ToString() + "): " + error.ErrorMessage); });
    }

    private string GetStasticName(MinigameType type)
    {
        return (isDebug ? "Test" : "") + type;
    }
}