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

    public MinigameType Type => type;
    public List<PlayerLeaderboardEntry> Entries
    {
        get => entries;
        internal set => entries = value;
    }
}

public class PlayFabManager : MonoSingleton<PlayFabManager>
{
    [SerializeField] private List<PlayFabLeaderboardElement> leaderboardEntries;

    [SerializeField] private int delayMinute = 5;
    [SerializeField] [ReadOnly] private bool isLoggedIn;

    private Coroutine routine;

    [Header("[DEBUG] 활성화 시 테스트 리더보드로 연결")] [SerializeField]
    private bool isDebug;

    public override void Init()
    {
        Login();
    }

    private void OnEnable()
    {
        isLoggedIn = false;
        ScoreManager.OnHighScoreChanged += SendLeaderboard;

        routine = StartCoroutine(LeaderBoardUpdateRoutine(delayMinute));
    }
    
    private void OnDisable()
    {
        isLoggedIn = false;
        ScoreManager.OnHighScoreChanged -= SendLeaderboard;
        
        StopCoroutine(routine);
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
            isLoggedIn = true;
            var profile = result.InfoResultPayload.PlayerProfile;
            if (profile != null)
                GameDatabase.NickName = profile.DisplayName;
            else
                UpdateDisplayName(GameDatabase.NickName);
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

    private IEnumerator LeaderBoardUpdateRoutine(int delay)
    {
        while (true)
        {
            if (!isLoggedIn)
            {
                yield return null;
                continue;
            }

            UpdateLeaderBoard();

            yield return new WaitForSeconds(delay * 60);
        }
    }

    public void UpdateLeaderBoard()
    {
        foreach (var element in leaderboardEntries)
        {
            var req = new GetLeaderboardRequest
            {
                StatisticName = GetStasticName(element.Type),
                StartPosition = 0,
                MaxResultsCount = 10
            };
            StartCoroutine(UpdateLeaderBoardElement(element, req));
        }
    }

    private static IEnumerator UpdateLeaderBoardElement(PlayFabLeaderboardElement element, GetLeaderboardRequest req)
    {
        var isComplete = false;
        PlayFabClientAPI.GetLeaderboard(req, result =>
        {
            element.Entries = result.Leaderboard;
            isComplete = true;
            Debug.Log("Get Leaderboard Success");
        }, error =>
        {
            Debug.LogError("Get Leaderboard Failed (" + error.Error.ToString() + "): " + error.ErrorMessage);
            throw new Exception(error.ErrorMessage);
        });

        yield return new WaitUntil((() => isComplete));
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