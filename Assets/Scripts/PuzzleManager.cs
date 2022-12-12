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
    private PuzzleDB _db;

    // [SerializeField] private List<PuzzleObjectArray> puzzleList;
    [SerializeField] private TextMeshProUGUI pieceCountTMP;

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
    
    public int GetPuzzle(int puzzleIndex)
    {
        return _db.completeState[puzzleIndex];
    }

    public bool PiecePuzzle(int puzzleIndex)
    {
        if (_db.pieceCount < 1) return false;
        if (GetPuzzle(puzzleIndex) >= 6) return true;
        
        Transform puzzles = GameObject.Find("Canvas").transform.GetChild(4).GetChild(0).GetChild(0);

        puzzles.GetChild(puzzleIndex).GetChild(1).GetChild(GetPuzzle(puzzleIndex)).GetComponent<Animator>().SetTrigger("Piece");

        var complNum = ++_db.completeState[puzzleIndex];
        var remainPzl = --_db.pieceCount;

        puzzles.GetChild(puzzleIndex).GetChild(4).GetComponent<TextMeshProUGUI>().text = complNum + " / 6";
        puzzles.parent.parent.GetChild(5).GetComponentInChildren<TextMeshProUGUI>().text = remainPzl.ToString();

        return true;
    }
}