using System;
using System.Collections.Generic;
using GameHeaven.UIUX;
using SharedLibs;
using SharedLibs.Score;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameDatabase : MonoSingleton<GameDatabase>
{
    [SerializeField] [FolderPath] private string saveFolderPath = "Save";
    [SerializeField] private string saveFileName = "savefile";

    [SerializeField] [ReadOnly] private string saveFilePath = "";

    [SerializeField] private GameDB db;

    [SerializeField] private bool isFirstTime;
    
    public GameDB DB => db;
    
    public bool IsFirstTime { get => isFirstTime; set => isFirstTime = value; }
    
    public static string NickName
    {
        get => Instance.db.nickName;
        set => Instance.db.nickName = value;
    }

    public override void Init()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, 60);
        
        saveFilePath = $"{Application.persistentDataPath}/{saveFolderPath}/{saveFileName}.json";

        db = LoadData();
        if (db == null)
        {
            isFirstTime = true;
            Debug.Log("First time");
            ResetData();
        }
    }
    
    public void SaveData(string path = "")
    {
        if (string.IsNullOrWhiteSpace(path)) path = saveFilePath;
        JsonUtil.SaveData(db, path);
    }

    public GameDB LoadData(string path = "")
    {
        if (string.IsNullOrWhiteSpace(path)) path = saveFilePath;
        return JsonUtil.LoadData<GameDB>(path);
    }

    public void ResetData()
    {
        db = new GameDB();
        db.ResetData();
    }

    protected override void SingletonDestroy()
    {
        SaveData();
    }
}

[Serializable]
public class GameDB
{
    public StoryDB storyDB;
    public PuzzleDB puzzleDB;
    public List<ScoreDB> scoreDBList;
    
    public string nickName;
    
    public void ResetData()
    {
        Debug.Log("ResetData");
        storyDB = new StoryDB() { unlockProgress = 0, lastViewedChapter = 0, viewPrologue = false, viewEpilogue = false};
        puzzleDB = new PuzzleDB() { pieceCount = 0, completeState = new List<int>() { 0, 0, 0, 0, 0 } };
        scoreDBList = new List<ScoreDB>()
        {
            new() { type = MinigameType.AttackGame }, new() { type = MinigameType.BingleGame },
            new() { type = MinigameType.CrashGame },
            new() { type = MinigameType.CrossGame }, new() { type = MinigameType.JumpGame },
            new() { type = MinigameType.PassGame },
            new() { type = MinigameType.PunctureGame }, new() { type = MinigameType.RunGame },
            new() { type = MinigameType.SpreadGame },
            new() { type = MinigameType.StickyGame }
        };

        nickName = GetRandomNickName();
    }

    private string GetRandomNickName()
    {
        return "팬치_이파리_" + Math.Abs(DateTime.UtcNow.GetHashCode()) % 1000000;
    }
}

[Serializable]
public class StoryDB
{
    public int unlockProgress;
    public int lastViewedChapter;
    
    public bool viewPrologue;
    public bool viewEpilogue;
}

[Serializable]
public class PuzzleDB
{
    public List<int> completeState;
    public int pieceCount;
}

[Serializable]
public class ScoreDB
{
    public MinigameType type;
    public int achievement;
}