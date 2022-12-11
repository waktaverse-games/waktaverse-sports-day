using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PuzzleObjectArray
{
    [SerializeField] private GameObject[] puzzleObjArr;
    [SerializeField] [ReadOnly] private bool isComplete;

    public GameObject[] PuzzleObjArr => puzzleObjArr;
    public bool IsComplete => isComplete;

    public void SetCompletePuzzle() => isComplete = true;
}

public class PuzzleManager : MonoBehaviour
{
    private static PuzzleDB _db;

    [SerializeField] private List<PuzzleObjectArray> puzzleList;

    private static int PuzzlePiece
    {
        get => _db.pieceCount;
        set => _db.pieceCount = value;
    }

    private void Awake()
    {
        if (_db == null)
        {
            _db = GameDatabase.Instance.DB.puzzleDB;
        }
    }

    private void Start()
    {
        print("PieceCount : " + _db.pieceCount);
        for (var i = 0; i < _db.completeState.Count; i++)
        {
            print("Puzzle" + i + " : " + _db.completeState[i]);
            for (var j = 0; j < _db.completeState[i]; j++)
            {
                GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetChild(0).GetChild(i).GetChild(1).GetChild(j)
                    .GetComponent<Animator>().SetTrigger("Piece");
            }
        }
    }

    public static void GetPuzzlePiece(int count = 1)
    {
        PuzzlePiece += count;
        print("Current Puzzle Piece: " + PuzzlePiece);
    }

    public static void UsePuzzlePiece(int count = 1)
    {
        PuzzlePiece -= count;
    }
    
    public static int GetPuzzle(int puzzleIndex)
    {
        return _db.completeState[puzzleIndex];
    }

    public static bool PiecePuzzle(int puzzleIndex)
    {
        if (PuzzlePiece < 1) return false;

        if (GetPuzzle(puzzleIndex) < 6)
        {
            GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetChild(0).GetChild(puzzleIndex).GetChild(1).GetChild(GetPuzzle(puzzleIndex))
                .GetComponent<Animator>().SetTrigger("Piece");

            _db.completeState[puzzleIndex]++;
            UsePuzzlePiece();
        }

        return true;
    }
}