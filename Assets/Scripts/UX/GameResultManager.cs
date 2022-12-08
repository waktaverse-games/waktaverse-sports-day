using GameHeaven.Root;
using GameHeaven.Temp;
using SharedLibs;
using SharedLibs.Score;
using TMPro;
using UnityEngine;

public class GameResultManager : MonoBehaviour
{
    public static MinigameType ResultGame;
    public static int ResultScore;
    
    public static bool IsResultScreen = false;
    
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI goPrevBtnText;
    
    [SerializeField] private GameObject successImageObj;
    [SerializeField] private GameObject failedImageObj;

    [SerializeField] private MinigameSceneData minigameData;

    private void Start()
    {
        var gameType = ResultGame;
        var score = ResultScore;
        
        var gameName = minigameData.GetGameName(gameType);
        gameNameText.text = gameName;
        if (GameManager.GameMode == GameMode.StoryMode)
        {
            var targetScore = ScoreManager.Instance.GetGameTargetScore(gameType);
            ShowStoryResultScreen(score, targetScore);
        }
        else
        {
            ShowMinigameResultScreen(score);
        }
    }

    private void OnEnable()
    {
        IsResultScreen = true;
    }

    private void OnDisable()
    {
        IsResultScreen = false;
    }

    public static void ShowResult(MinigameType type, int score)
    {
        if (IsResultScreen) return;
        
        ResultGame = type;
        ResultScore = score;
        
        var target = ScoreManager.Instance.GetGameTargetScore(type);
        // next story unlock when success
        if (score >= target && StoryManager.Instance.SelectStoryIndex == StoryManager.Instance.UnlockProgress)
        {
            StoryManager.Instance.UnlockNext();
        }

        SceneLoader.AddSceneAsync("ResultScene");
    }

    private void ShowStoryResultScreen(int score, int target)
    {
        scoreText.text = score + " / " + target;
        var success = score >= target;
        successImageObj.SetActive(success);
        failedImageObj.SetActive(!success);
        goPrevBtnText.text = "스토리모드";
    }
    private void ShowMinigameResultScreen(int score)
    {
        successImageObj.SetActive(false);
        failedImageObj.SetActive(false);
        scoreText.text = score.ToString();
        goPrevBtnText.text = "미니게임으로";
    }
    
    // Scene Load (Temporary)

    public void LoadMain()
    {
        SceneLoader.LoadSceneAsync("ModeSelectScene");
    }
    public void LoadPrevMode()
    {
        if (GameManager.GameMode == GameMode.StoryMode)
        {
            if (StoryManager.Instance.IsAllUnlock && !StoryManager.Instance.ViewEpilogue)
            {
                SceneLoader.LoadSceneAsync("EpilogueStoryScene");
            }
            else
            {
                SceneLoader.LoadSceneAsync("StoryMenuScene");
            }
        }
        else
        {
            SceneLoader.LoadSceneAsync("MinigameMenuScene");
        }
    }
}