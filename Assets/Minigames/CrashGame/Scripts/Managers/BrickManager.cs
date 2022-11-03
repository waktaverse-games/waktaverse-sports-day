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
        public Brick itemBrickPrefab;

        private Queue<List<Brick>> brickMap;

        public List<Coin> coinPrefabList;
        public List<Item> itemPrefabList;

        private Vector2 currentBrickPosition;
        private Vector3 brickTranslateDown;
        private Vector2 brickCenterPosition;

        public const float BrickStartXPosition = -3.75f;
        public const float BrickStartYPosition = 5f;
        public const float BrickWidth = .9375f;
        public const float BrickHeight = .5f;

        public Queue<List<Brick>> BrickMap
        {
            get { return brickMap; }
            private set { brickMap = value; }
        }

        // BasicBrick 계열 블럭 색깔
        [HideInInspector]
        public Color[] brickColorArray;

        private void Awake()
        {
            brickTranslateDown = new Vector3(0, -BrickHeight, 0);
            brickCenterPosition = new Vector2(BrickWidth / 2, -BrickHeight / 2);
            brickColorArray = new Color[] { new Color32(138, 43, 226, 255), new Color32(240, 169, 87, 255), new Color32(0, 0, 128, 255), new Color32(128, 0, 128, 255), new Color32(70, 126, 198, 255), new Color32(133, 172, 32, 255) };
        }

        private void Start()
        {
            
        }

        public IEnumerator BlockLineAddLoop(float initialInterval, float finalInterval)
        {
            float interval = initialInterval;
            while (true)
            {
                yield return new WaitForSeconds(interval / 2);
                Debug.Log($"{interval / 2} seconds passed!");
                yield return new WaitForSeconds(interval / 2);
                Debug.Log($"{interval} seconds passed!");
                AddBrickLineInMap();
                if (interval > finalInterval) interval *= 0.95f;
                else interval = finalInterval;
            }
        }

        public bool CheckOuterLineDestroyed(bool addNewLine)
        {
            // 맨 아래 행 블럭이 전부 파괴되었는지 체크
            bool allBrickOff = true;
            if (brickMap.Count == 0) 
            {
                PerfectBonus();
            } 
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

                if (addNewLine) AddBrickLineInMap();
                CheckOuterLineDestroyed(addNewLine); // 그다음 행도 블럭이 전부 파괴되어있었는지 체크
            }

            return allBrickOff;
        }

        private void PerfectBonus()
        {
            //TODO Effect
            StartCoroutine(GameManager.Instance.UI.PerfectBonus());
            GameManager.Instance.AddScore(500);
            ResetBricks();
        }

        private IEnumerator AddLineLoop(int loopNumber)
        {
            for (int i = 0; i < loopNumber; i++)
            {
                yield return new WaitForSeconds(.1f);
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
            StartCoroutine(AddLineLoop(5));
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
                //// Test
                //brickLine.Add(AddBrick(hardBrickPrefab, currentBrickPosition + brickCenterPosition));
                switch (Random.Range(0, 24))
                {
                    case 0:
                    case 1:
                    case 2:
                        // 단단한 벽돌
                        brickLine.Add(AddBrick(hardBrickPrefab, currentBrickPosition + brickCenterPosition));
                        break;
                    case 3:
                    case 4:
                        // 공 추가 벽돌
                        brickLine.Add(AddBrick(ballBrickPrefab, currentBrickPosition + brickCenterPosition));
                        break;
                    case 5:
                        // 아이템 벽돌
                        ItemBrick newItemBrick = (ItemBrick)AddBrick(itemBrickPrefab, currentBrickPosition + brickCenterPosition);
                        int itemIndex = Random.Range(0, itemPrefabList.Count);
                        newItemBrick.ItemPrefab = itemPrefabList[itemIndex];
                        brickLine.Add(newItemBrick);
                        break;
                    default:
                        brickLine.Add(AddBrick(basicBrickPrefab, currentBrickPosition + brickCenterPosition));
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
