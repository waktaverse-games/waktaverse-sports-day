using System.Collections;
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
    
    [SerializeField] private GameObject resultScreen;
    
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI goPrevBtnText;
    
    [SerializeField] private GameObject successImageObj;
    [SerializeField] private GameObject failedImageObj;

    [SerializeField] private MinigameSceneData minigameData;
    
    [SerializeField] private float waitShowResultTime = 2f;

    private void Start()
    {
        resultScreen.SetActive(false);
        StartCoroutine(ShowResultScreen(waitShowResultTime));
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

    private IEnumerator ShowResultScreen(float waitSec)
    {
        yield return new WaitForSeconds(waitSec);
        resultScreen.SetActive(true);
        
        var gameType = ResultGame;
        var score = ResultScore;
        
        var gameName = minigameData.GetGameName(gameType);
        gameNameText.text = gameName;
        if (GameManager.GameMode == GameMode.StoryMode)
        {
            var targetScore = ScoreManager.Instance.GetGameTargetScore(gameType);
            
            ShowStoryResultScreen(score, targetScore);
            goPrevBtnText.text = (StoryManager.Instance.IsAllUnlock && !StoryManager.Instance.ViewEpilogue) ? "다음으로" : "다시하기";
        }
        else
        {
            ShowMinigameResultScreen(score);
        }
    }

    private void ShowStoryResultScreen(int score, int target)
    {
        scoreText.text = score + " / " + target;
        var success = score >= target;
        successImageObj.SetActive(success);
        failedImageObj.SetActive(!success);
    }
    private void ShowMinigameResultScreen(int score)
    {
        successImageObj.SetActive(false);
        failedImageObj.SetActive(false);
        scoreText.text = score.ToString();
    }
    
    // Scene Load (Temporary)

    public void Return()
    {
        if (GameManager.GameMode == GameMode.StoryMode)
        {
            SceneLoader.LoadSceneAsync("StoryMenuScene");
        }
        else
        {
            SceneLoader.LoadSceneAsync("MinigameMenuScene");
        }
    }
    public void Replay()
    {
        if (StoryManager.Instance.IsAllUnlock && !StoryManager.Instance.ViewEpilogue)
        {
            SceneLoader.LoadSceneAsync("EpilogueStoryScene");
        }
        else
        {
            var sceneName = minigameData.GetSceneName(ResultGame);
            var illustSprite = minigameData.GetIllustSprite(ResultGame);
            LoadingSceneManager.LoadScene(sceneName, illustSprite);
        }
    }
}