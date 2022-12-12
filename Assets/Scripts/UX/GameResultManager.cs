using System.Collections;
using GameHeaven.Root;
using GameHeaven.Temp;
using SharedLibs;
using SharedLibs.Score;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject puzzleRoot;
    [SerializeField] private Image[] puzzleInnerImgs;

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
        
        var gameName = minigameData.GetGameName(ResultGame);
        gameNameText.text = gameName;
        if (GameManager.GameMode == GameMode.StoryMode)
        {
            var targetScore = ScoreManager.Instance.GetGameTargetScore(ResultGame);
            
            ShowStoryResultScreen(ResultScore, targetScore);
            goPrevBtnText.text = (StoryManager.Instance.IsAllUnlock && !StoryManager.Instance.ViewEpilogue) ? "다음으로" : "다시하기";
        }
        else
        {
            ShowMinigameResultScreen(ResultScore);
        }
    }

    private void ShowStoryResultScreen(int score, int target)
    {
        scoreText.text = score + " / " + target;
        var success = score >= target;
        successImageObj.SetActive(success);
        failedImageObj.SetActive(!success);
        puzzleRoot.SetActive(false);
    }
    private void ShowMinigameResultScreen(int score)
    {
        successImageObj.SetActive(false);
        failedImageObj.SetActive(false);
        puzzleRoot.SetActive(true);
        ShowPuzzlePiece();
        
        scoreText.text = score.ToString();
    }
    
    private void ShowPuzzlePiece()
    {
        var havePiece = ScoreManager.Instance.GetGameAchievement(ResultGame);
        var resultPiece = ScoreManager.Instance.SetGameAchievement(ResultGame, ResultScore);
        
        Debug.Log("Achievement : " + havePiece + " -> " + resultPiece);
        
        havePiece = Mathf.Min(havePiece, puzzleInnerImgs.Length);
        resultPiece = Mathf.Min(resultPiece, puzzleInnerImgs.Length);

        // 얻지 못한 퍼즐 조각
        for (var i = resultPiece; i < puzzleInnerImgs.Length; i++)
        {
            var color = puzzleInnerImgs[i].color;
            color.a = 0.0f;
            puzzleInnerImgs[i].color = color;
        }
        // 이미 얻은 퍼즐 조각
        for (var i = 0; i < havePiece; i++)
        {
            var color = puzzleInnerImgs[i].color;
            color.a = 0.8f;
            puzzleInnerImgs[i].color = color;
        }
        // 새로 얻은 퍼즐 조각
        for (var i = havePiece; i < resultPiece; i++)
        {
        }
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