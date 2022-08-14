using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class BrickManager : MonoBehaviour
    {
        [SerializeField]
        private Transform brickParent;

        public Brick basicBrickPrefab;
        public Brick hardBrickPrefab;
        public Brick ballBrickPrefab;

        private Queue<List<Brick>> brickMap;

        public List<Coin> coinPrefabList;
        public List<Item> itemPrefabList;

        private Vector2 currentBrickPosition;
        private Vector3 brickTranslateDown;

        public const float BrickStartXPosition = -3.75f;
        public const float BrickStartYPosition = 5f;
        public const float BrickWidth = .9375f;
        public const float BrickHeight = .5f;

        private void Awake()
        {
            brickTranslateDown = new Vector3(0, -BrickHeight, 0);
        }

        private void Start()
        {
        }

        public void CheckOuterLineDestroyed()
        {
            // 맨 아래 행 블럭이 전부 파괴되었는지 체크
            bool allBrickOff = true;
            if (brickMap.Count == 0) return;
            foreach (Brick brick in brickMap.Peek())
            {
                if (brick.gameObject.activeSelf) allBrickOff = false;
            }

            if (allBrickOff)
            {
                var destroyList = brickMap.Dequeue();
                foreach (Brick brick in destroyList)
                {
                    Destroy(brick.gameObject);
                }

                AddBrickLineInMap();
                CheckOuterLineDestroyed(); // 그다음 행도 블럭이 전부 파괴되어있었는지 체크
            }
        }

        public void ResetBricks()
        {
            if (brickMap != null)
            {
                foreach (var brickLine in brickMap)
                {
                    foreach (var brick in brickLine)
                    {
                        Destroy(brick.gameObject);
                    }
                }
            }

            brickMap = new Queue<List<Brick>>();
            for (int i = 0; i < 5; i++)
            {
                AddBrickLineInMap();
            }
        }

        public void AddBrickLineInMap()
        {
            foreach (var brickLine in brickMap)
            {
                MoveBrickLineDown(brickLine);
            }
            brickMap.Enqueue(AddBrickLine());
        }

        private List<Brick> AddBrickLine()
        {
            List<Brick> brickLine = new List<Brick>();
            currentBrickPosition = new Vector2(BrickStartXPosition, BrickStartYPosition);
            for (int i = 0; i < 8; i++)
            {
                
                switch (Random.Range(0, 8))
                {
                    case 0:
                        brickLine.Add(AddBrick(hardBrickPrefab, currentBrickPosition));
                        break;
                    case 1:
                        brickLine.Add(AddBrick(ballBrickPrefab, currentBrickPosition));
                        break;
                    default:
                        brickLine.Add(AddBrick(basicBrickPrefab, currentBrickPosition));
                        break;
                }
                currentBrickPosition.x += BrickWidth;
            }
            return brickLine;
        }

        private void MoveBrickLineDown(List<Brick> brickLine)
        {
            foreach (var brick in brickLine)
            {
                brick.transform.Translate(brickTranslateDown);
            }
        }

        private Brick AddBrick(Brick brickPrefab, Vector2 position)
        {
            var newBrick = Instantiate(brickPrefab, position, Quaternion.identity);
            newBrick.transform.SetParent(brickParent);
            return newBrick;
        }


    }

}
