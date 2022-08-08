using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class BrickManager : MonoBehaviour
    {
        public GameObject basicBrickPrefab;
        public Transform brickParent;

        public List<Brick> bricks;

        private Vector2 currentBrickPosition;

        const float BrickStartXPosition = -3.75f;
        const float BrickWidth = .9375f;
        const float BrickHeight = .5f;

        private void Awake()
        {
            InitializeBricks();
        }

        public void InitializeBricks()
        {
            currentBrickPosition = new Vector2(BrickStartXPosition, 5.5f);
            for (int i = 0; i < 40; i++)
            {
                AddBrick(basicBrickPrefab);
            }
        }

        public void AddBrick(GameObject brick)
        {
            if (bricks.Count % 8 != 0)
            {
                currentBrickPosition = new Vector3(currentBrickPosition.x + BrickWidth, currentBrickPosition.y, 0);
            }
            else
            {
                currentBrickPosition = new Vector3(BrickStartXPosition, currentBrickPosition.y - BrickHeight, 0);
            }
            GameObject newBrick = GameObject.Instantiate(brick, brickParent);
            newBrick.transform.position = currentBrickPosition;
            bricks.Add(brick.GetComponent<Brick>());

        }

    }
}

