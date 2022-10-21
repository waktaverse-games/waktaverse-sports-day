using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class BackgroundScrolling : MonoBehaviour
    {
        [SerializeField] float speed;
        int startIndex = 0;
        int endIndex = 2;
        public Transform[] sprites;

        float viewHeight;

        private void Awake()
        {
            viewHeight = 10; // Camera.main.orthographicSize * 2;
        }
        void Update()
        {
            speed = GameSpeedController.instance.speed;

            Vector3 curPos = transform.position;
            Vector3 nextPos = Vector3.up * speed * Time.deltaTime;
            transform.position = curPos + nextPos;

            if (sprites[endIndex].position.y >= viewHeight)
            {
                Vector3 backSpritePos = sprites[startIndex].localPosition;

                sprites[endIndex].transform.localPosition = backSpritePos + Vector3.down * viewHeight;

                int startIndexSave = startIndex;
                startIndex = endIndex;
                endIndex = (startIndexSave + 1 == sprites.Length) ? 0 : startIndexSave + 1;
            }
        }
    }
}