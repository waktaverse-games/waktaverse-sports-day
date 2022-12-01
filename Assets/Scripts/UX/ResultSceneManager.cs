using System;
using System.Collections.Generic;
using GameHeaven.Root;
using GameHeaven.UIUX;
using SharedLibs;
using SharedLibs.Score;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ResultSceneData
{
    [SerializeField] private MinigameType gameType;
    [SerializeField] private string gameName;

    public MinigameType GameType => gameType;
    public string GameName => gameName;
}

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI goPrevBtnText;
    
    [SerializeField] private GameObject successImageObj;
    [SerializeField] private GameObject failedImageObj;

    [SerializeField] private ResultSceneData[] gameNameArr;
    private static Dictionary<MinigameType, string> _gameNameDic;

    private static MinigameType gameType;

    private void Awake()
    {
        if (_gameNameDic == null)
        {
            _gameNameDic = new Dictionary<MinigameType, string>();
            foreach (var data in gameNameArr)
            {
                _gameNameDic.Add(data.GameType, data.GameName);
            }
        }
    }

    private void Start()
    {
        gameNameText.text = _gameNameDic[gameType];

        SetResultScreenUI(GameManager.GameMode);
    }

    private void SetResultScreenUI(GameMode mode)
    {
        var score = ScoreManager.Instance.GetGameScore(gameType);
        if (mode == GameMode.StoryMode)
        {
            var target = ScoreManager.Instance.GetGameTargetScore(gameType);
            scoreText.text = score + " / " + target;

            var succeeded = score >= target;

            // next story unlock when success
            if (succeeded && StoryManager.Instance.SelectStoryIndex == StoryManager.Instance.UnlockProgress)
            {
                StoryManager.Instance.UnlockNext();
            }
        
            successImageObj.SetActive(succeeded);
            failedImageObj.SetActive(!succeeded);

            goPrevBtnText.text = "스토리모드";
        }
        else
        {
            scoreText.text = score.ToString();
            
            goPrevBtnText.text = "미니게임으로";
        }
    }

    public static void ShowResult(MinigameType type)
    {
        gameType = type;
        SceneLoader.AddSceneAsync("ResultScene");
    }
    
    // Scene Load (Temporary)

    public void LoadMain()
    {
        SceneLoader.LoadSceneAsync("ModeSelectScene");
    }
    public void LoadPrevMode()
    {
        SceneLoader.LoadSceneAsync(GameManager.GameMode == GameMode.StoryMode ? "StoryMenuScene" : "MinigameMenuScene");
    }
}