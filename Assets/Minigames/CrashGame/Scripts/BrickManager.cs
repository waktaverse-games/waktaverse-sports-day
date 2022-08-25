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
        public Brick itemTestBrickPrefab;

        private Queue<List<Brick>> brickMap;

        public List<Coin> coinPrefabList;
        public List<Item> itemPrefabList;

        private Vector2 currentBrickPosition;
        private Vector3 brickTranslateDown;

        public const float BrickStartXPosition = -3.75f;
        public const float BrickStartYPosition = 5f;
        public const float BrickWidth = .9375f;
        public const float BrickHeight = .5f;

        // BasicBrick �迭 �� ����
        public Color[] brickColorArray;

        private void Awake()
        {
            brickTranslateDown = new Vector3(0, -BrickHeight, 0);
            brickColorArray = new Color[] { new Color32(138, 43, 226, 255), new Color32(240, 169, 87, 255), new Color32(0, 0, 128, 255), new Color32(128, 0, 128, 255), new Color32(70, 126, 198, 255), new Color32(133, 172, 32, 255) };
        }

        private void Start()
        {
        }

        public void CheckOuterLineDestroyed()
        {
            // �� �Ʒ� �� ���� ���� �ı��Ǿ����� üũ
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
                CheckOuterLineDestroyed(); // �״��� �൵ ���� ���� �ı��Ǿ��־����� üũ
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
                
                switch (Random.Range(0, 10))
                {
                    case 0:
                        brickLine.Add(AddBrick(hardBrickPrefab, currentBrickPosition));
                        break;
                    case 1:
                        brickLine.Add(AddBrick(ballBrickPrefab, currentBrickPosition));
                        break;
                    case 2:
                        brickLine.Add(AddBrick(itemTestBrickPrefab, currentBrickPosition));
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
