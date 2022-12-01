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
        for (var i = 0; i < puzzleList.Count; i++)
        {
            var state = _db.completeState[i];
            var count = 0;
            for (var j = 0; j < 6; j++)
            {
                if ((state & (1 << j)) == 1)
                {
                    puzzleList[i].PuzzleObjArr[j].SetActive(true);
                    count++;
                }
            }

            if (count >= 6)
            {
                puzzleList[i].SetCompletePuzzle();
            }
        }
    }

    public static void GetPuzzle(int count = 1)
    {
        PuzzlePiece += count;
        print("Current Puzzle Piece: " + PuzzlePiece);
    }

    public void UsePuzzle(int count = 1)
    {
        PuzzlePiece -= count;
    }

    public bool PiecePuzzle(int puzzleIndex, int pieceIndex)
    {
        if (!IsPlaceablePiece(puzzleIndex, pieceIndex)) return false;
        
        _db.completeState[puzzleIndex] |= 1 << pieceIndex;
        UsePuzzle();
        
        // 여기에 애니메이션 재생 넣기

        return true;
    }

    public bool IsPlaceablePiece(int puzzleIndex, int pieceIndex)
    {
        var beforePiece = _db.completeState[puzzleIndex];
        var afterPiece = beforePiece | 1 << pieceIndex;

        return beforePiece != afterPiece;
    }
}