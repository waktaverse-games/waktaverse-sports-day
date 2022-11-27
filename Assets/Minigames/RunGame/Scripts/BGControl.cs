using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{
    public class BGControl : MonoBehaviour
    {

        public int startIndex;
        public int endIndex;
        public GameManager gameManager;
        public Transform[] sprites;

        // Update is called once per frame
        void Update()
        {
            if (gameManager.gameStop == false && gameManager.gameStart == true)
            {
                Vector3 curPos = transform.position;
                Vector3 nextPos = Vector3.down * gameManager.wallSpeed * Time.deltaTime;
                transform.position = curPos + nextPos;

                if (sprites[endIndex].position.y < -32.5f)
                {
                    Vector3 backSpritePos = sprites[startIndex].localPosition;
                    Vector3 frontSpritePos = sprites[endIndex].localPosition;
                    sprites[endIndex].transform.localPosition = backSpritePos + Vector3.up * 27;

                    int startIndexSave = startIndex;
                    startIndex = endIndex;
                    endIndex = (startIndexSave - 1 == -1) ? sprites.Length - 1 : startIndexSave - 1;
                }
            }
        }
    }
}