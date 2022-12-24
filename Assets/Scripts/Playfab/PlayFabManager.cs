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

    [SerializeField] private int maxLeaderboardResultCount = 20;
    
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
        ScoreManager.OnHighScoreChanged += PostLeaderboard;
    }
    
    private void OnDisable()
    {
        playFabId = "";
        ScoreManager.OnHighScoreChanged -= PostLeaderboard;
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
                PostDisplayName(GameDatabase.NickName, null, null);

            UpdateLeaderBoard();
        }, error => Debug.LogError("Login Failed (" + error.Error.ToString() + "): " + error.ErrorMessage));
    }

    public void PostLeaderboard(MinigameType type, int score)
    {
        leaderboardEntries.Find(ent => ent.Type == type).PlayerEntry.StatValue = score;
        
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

    public void PostDisplayName(string displayName, Action onSuccess, Action onFailed)
    {
        GameDatabase.NickName = displayName;

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = displayName },
            result =>
            {
                onSuccess?.Invoke();
                Debug.Log("UpdateUserTitleDisplayName Success");
            },
            error =>
            {
                onFailed?.Invoke();
                Debug.LogError("UpdateUserTitleDisplayName Error (" + error.Error.ToString() + "): " +
                               error.ErrorMessage);
            });
    }

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
                MaxResultsCount = maxLeaderboardResultCount
            };
            var playerReq = new GetLeaderboardAroundPlayerRequest
            {
                StatisticName = GetStasticName(element.Type),
                MaxResultsCount = 1
            };
            StartCoroutine(UpdateRequestLeaderBoard(element, leaderboardReq, playerReq));
        }
    }

    private IEnumerator UpdateRequestLeaderBoard(PlayFabLeaderboardElement element, GetLeaderboardRequest leaderboardReq, GetLeaderboardAroundPlayerRequest playerReq)
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
            Debug.Log(entry.PlayFabId + " / " + entry.DisplayName);
            if (entry.PlayFabId == playFabId)
            {
                element.PlayerEntry = entry;
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

    public List<PlayerLeaderboardEntry> GetLeaderboardData(MinigameType type)
    {
        var element = leaderboardEntries.Find(t => t.Type == type);
        return element.Entries;
    }

    public PlayerLeaderboardEntry GetLeaderBoardAroundPlayerData(MinigameType type)
    {
        var element = leaderboardEntries.Find(t => t.Type == type);
        return element.PlayerEntry;
    }

    private string GetStasticName(MinigameType type)
    {
        return (isDebug ? "Test" : "") + type;
    }
}