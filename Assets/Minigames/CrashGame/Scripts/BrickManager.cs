using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class BrickManager : MonoBehaviour
    {
        public Transform brickParent;

        public GameObject basicBrickPrefab;
        public GameObject hardBrickPrefab;

        public List<Brick> mapBricks;

        private Vector2 currentBrickPosition;

        const float BrickStartXPosition = -3.75f;
        const float BrickWidth = .9375f;
        const float BrickHeight = .5f;

        private void Awake()
        {
            ResetBricks();
        }

        public void ResetBricks()
        {
            // Å×½ºÆ®
            currentBrickPosition = new Vector2(BrickStartXPosition, 5.5f);
            for (int i = 0; i < 40; i++)
            {
                switch (Random.Range(0, 4))
                {
                    case 0:
                        AddBrick(hardBrickPrefab);
                        break;
                    default:
                        AddBrick(basicBrickPrefab);
                        break;
                }
                
            }
        }

        public void AddBrick(GameObject brick)
        {
            if (mapBricks.Count % 8 != 0)
            {
                currentBrickPosition = new Vector3(currentBrickPosition.x + BrickWidth, currentBrickPosition.y, 0);
            }
            else
            {
                currentBrickPosition = new Vector3(BrickStartXPosition, currentBrickPosition.y - BrickHeight, 0);
            }
            GameObject newBrick = GameObject.Instantiate(brick, brickParent);
            newBrick.transform.position = currentBrickPosition;
            mapBricks.Add(brick.GetComponent<Brick>());

        }

    }

}
