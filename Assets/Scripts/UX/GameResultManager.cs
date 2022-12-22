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
    [SerializeField] private GameObject returnBtnRed, returnBtnGray;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private GameObject epilogueBtn;
    [SerializeField] private GameObject retryBtn;
    
    [SerializeField] private GameObject successImageObj;
    [SerializeField] private GameObject failedImageObj;
    [SerializeField] private GameObject puzzleRoot;
    [SerializeField] private GameObject[] puzzlePieces;

    [SerializeField] private AudioSource pieceGetSound;

    [SerializeField] private MinigameSceneData minigameData;
    
    [SerializeField] private float waitShowResultTime = 2f;

    private void Start()
    {
        resultScreen.SetActive(false);
        StartCoroutine(ShowResultScreen(waitShowResultTime));
        pieceGetSound.volume = SoundManager.Instance.SFXVolume;
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
        
        LoggerSystem.Log(GameManager.GameMode.ToString(), type.ToString(), score.ToString());
        
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
            if (StoryManager.Instance.IsAllUnlock && !StoryManager.Instance.ViewEpilogue)
            {
                returnBtnRed.SetActive(false);
                returnBtnGray.SetActive(false);
                retryBtn.SetActive(false);
                epilogueBtn.SetActive(true);
            }
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
        returnBtnRed.SetActive(false);
        returnBtnGray.SetActive(!success);
        nextBtn.SetActive(success);

        puzzleRoot.SetActive(false);

        if (success && GameDatabase.Instance.DB.storyDB.lastViewedChapter < 9) GameDatabase.Instance.DB.storyDB.lastViewedChapter++;
    }
    private void ShowMinigameResultScreen(int score)
    {
        successImageObj.SetActive(false);
        failedImageObj.SetActive(false);

        returnBtnRed.SetActive(true);
        returnBtnGray.SetActive(false);
        nextBtn.SetActive(false);

        puzzleRoot.SetActive(true);
        ShowPuzzlePiece();
        
        scoreText.text = score.ToString();
    }
    
    private void ShowPuzzlePiece()
    {
        var havePiece = ScoreManager.Instance.GetGameAchievement(ResultGame);
        var resultPiece = ScoreManager.Instance.SetGameAchievement(ResultGame, ResultScore);
        
        Debug.Log("Achievement : " + havePiece + " -> " + resultPiece);
        
        havePiece = Mathf.Min(havePiece, puzzlePieces.Length);
        resultPiece = Mathf.Min(resultPiece, puzzlePieces.Length);

        // Mouse Over Event Text
        var goals = ScoreManager.Instance.GetRewardGoals(ResultGame);
        for (int i = 0; i < 3; i++)
        {
            puzzlePieces[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = goals[i] + "점 이상";
        }

        // 이미 얻은 퍼즐 조각
        for (var i = 0; i < havePiece; i++)
        {
            puzzlePieces[i].GetComponent<Animator>().SetTrigger("Aquired");
        }

        // 새로 얻은 퍼즐 조각
        StartCoroutine(PieceGetSequentially(havePiece, resultPiece));
    }

    IEnumerator PieceGetSequentially(int havePiece, int resultPiece)
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        // 새로 얻은 퍼즐 조각
        for (var i = havePiece; i < resultPiece; i++)
        {
            pieceGetSound.Play();
            puzzleRoot.transform.GetChild(i).GetComponent<Animator>().SetTrigger("PieceGet");
            yield return wait;
        }
    }
    
    // Scene Load (Temporary)

    public void Return()
    {
        FindObjectOfType<UIBGM>().OnUIBGM();
        SceneLoader.LoadSceneAsync(GameManager.GameMode == GameMode.StoryMode ? "StoryMenuScene" : "MinigameMenuScene");
    }
    public void Replay()
    {
        var sceneName = minigameData.GetSceneName(ResultGame);
        var illustSprite = minigameData.GetIllustSprite(ResultGame);
        LoadingSceneManager.LoadScene(sceneName, illustSprite);
    }
    public void Epilogue()
    {
        SceneLoader.LoadSceneAsync("EpilogueStoryScene");
    }
}