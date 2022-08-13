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

        [System.Obsolete]
        public List<Brick> mapBricks;

        public Queue<List<Brick>> brickMap;

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
            ResetBricks();
        }

        [System.Obsolete]
        public void OldResetBricks()
        {
            // Å×½ºÆ®
            foreach(Brick brick in mapBricks)
            {
                Destroy(brick.gameObject);
            }
            mapBricks = new List<Brick>();
            currentBrickPosition = new Vector2(BrickStartXPosition, 5.5f);
            for (int i = 0; i < 40; i++)
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        OldAddBrick(hardBrickPrefab.gameObject);
                        break;
                    default:
                        OldAddBrick(basicBrickPrefab.gameObject);
                        break;
                }
                
            }
        }

        public void CheckOuterLineDestroyed()
        {
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

        [System.Obsolete]
        public void OldAddBrick(GameObject brick)
        {
            // Deprecated
            if (mapBricks.Count % 8 != 0)
            {
                currentBrickPosition = new Vector3(currentBrickPosition.x + BrickWidth, currentBrickPosition.y, 0);
            }
            else
            {
                currentBrickPosition = new Vector3(BrickStartXPosition, currentBrickPosition.y - BrickHeight, 0);
            }
            GameObject newBrick = Instantiate(brick, brickParent);
            newBrick.transform.position = currentBrickPosition;
            mapBricks.Add(brick.GetComponent<Brick>());

        }

    }

}
