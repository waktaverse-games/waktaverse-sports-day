using System;
using SharedLibs;
using SharedLibs.Score;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject StoryModeResultObject;
    [SerializeField] private TextMeshProUGUI storyGameNameText;
    [SerializeField] private TextMeshProUGUI storyScoreText;
    
    [SerializeField] private GameObject MinigameModeResultObject;
    [SerializeField] private TextMeshProUGUI minigameGameNameText;
    [SerializeField] private TextMeshProUGUI minigameScoreText;

    private static MinigameType gameType;
    private static bool IsStoryModeResult;

    private void Awake()
    {
        StoryModeResultObject.SetActive(IsStoryModeResult);
        MinigameModeResultObject.SetActive(!IsStoryModeResult);
    }

    private void Start()
    {
        var score = ScoreManager.Instance.GetGameScore(gameType);
        
        if (IsStoryModeResult)
        {
            storyGameNameText.text = gameType.ToString();
            storyScoreText.text = score + " / " + 9999;
        }
        else
        {
            minigameGameNameText.text = gameType.ToString();
            minigameScoreText.text = score.ToString();
        }
    }

    public static void SetResultType(bool isStoryMode)
    {
        IsStoryModeResult = isStoryMode;
    }

    public static void ShowResult(MinigameType type)
    {
        gameType = type;
        SceneLoader.AddSceneAsync("ResultScene");
    }

    public void GoToMain()
    {
        SceneLoader.LoadSceneAsync("ModeSelectScene");
    }
    public void GoToStory()
    {
        SceneLoader.LoadSceneAsync("StoryMenuScene");
    }
    public void GoToMinigame()
    {
        SceneLoader.LoadSceneAsync("MinigameMenuScene");
    }
}