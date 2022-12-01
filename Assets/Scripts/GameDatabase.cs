using System;
using System.Collections.Generic;
using GameHeaven.UIUX;
using SharedLibs;
using SharedLibs.Score;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameDatabase : MonoSingleton<GameDatabase>
{
    [SerializeField] [FolderPath] private string jsonFolderPath = "Resources/Save";
    [SerializeField] private string jsonFileName = "game-save";

    [SerializeField] [ReadOnly] private string unityJsonFilePath = "";

    [SerializeField] private GameDB db;

    public GameDB DB => db;

    public override void Init()
    {
        unityJsonFilePath = $"{Application.dataPath}/{jsonFolderPath}/{jsonFileName}.json";

        db = JsonUtil.LoadData<GameDB>(unityJsonFilePath);
        if (db == null)
        {
            db = new GameDB();
            db.ResetData();
        }
    }

    protected override void SingletonDestroy()
    {
        JsonUtil.SaveData(db, unityJsonFilePath);
    }
}

[Serializable]
public class StoryDB
{
    public int unlockProgress;
    public int viewProgress;
}

[Serializable]
public class PuzzleDB
{
    public List<int> completeState;
    public int pieceCount;

    public PuzzleDB()
    {
        completeState = new List<int>() { 0, 0, 0, 0, 0 };
    }
}

[Serializable]
public class ScoreDB
{
    public MinigameType type;
    public int highScore;
    public int achievement;
}

[Serializable]
public class GameDB : ILocalData
{
    public StoryDB storyDB;
    public PuzzleDB puzzleDB;
    public List<ScoreDB> scoreDBList;
    
    public string nickName;
    
    public void ResetData()
    {
        storyDB = new StoryDB() { unlockProgress = 0, viewProgress = -1 };
        puzzleDB = new PuzzleDB() { pieceCount = 0 };
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
        return "팬치 이파리 " + Math.Abs(DateTime.UtcNow.GetHashCode()) % 1000000;
    }
}

public interface ILocalData
{
    void ResetData();
}