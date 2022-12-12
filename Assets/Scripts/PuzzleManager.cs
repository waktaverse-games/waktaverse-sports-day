using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

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
    [SerializeField] private TextMeshProUGUI pieceCountTMP;

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
        Transform puzzles = GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetChild(0);

        pieceCountTMP.text = _db.pieceCount.ToString();
        for (var i = 0; i < _db.completeState.Count; i++)
        {
            for (var j = 0; j < _db.completeState[i]; j++)
            {
                puzzles.transform.GetChild(i).GetChild(1).GetChild(j).GetComponent<Animator>().SetTrigger("Piece");
                puzzles.transform.GetChild(i).GetChild(4).GetComponent<TextMeshProUGUI>().text = _db.completeState[i] + " / 6";
            }
        }
    }

    public static void GetPuzzlePiece(int count = 1)
    {
        PuzzlePiece += count;
        print("Current Puzzle Piece: " + PuzzlePiece);
    }

    public static int UsePuzzlePiece(int count = 1)
    {
        return PuzzlePiece -= count;
    }
    
    public static int GetPuzzle(int puzzleIndex)
    {
        return _db.completeState[puzzleIndex];
    }
    
    public static int SetPuzzle(int puzzleIndex)
    {
        return ++_db.completeState[puzzleIndex];
    }

    public static bool PiecePuzzle(int puzzleIndex)
    {
        if (PuzzlePiece < 1) return false;

        if (GetPuzzle(puzzleIndex) < 6)
        {
            Transform puzzles = GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetChild(0);

            puzzles.GetChild(puzzleIndex).GetChild(1).GetChild(GetPuzzle(puzzleIndex)).GetComponent<Animator>().SetTrigger("Piece");

            var complNum = SetPuzzle(puzzleIndex);
            var remainPzl = UsePuzzlePiece();

            puzzles.GetChild(puzzleIndex).GetChild(4).GetComponent<TextMeshProUGUI>().text = complNum + " / 6";
            puzzles.parent.parent.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = remainPzl.ToString();
        }

        return true;
    }
}