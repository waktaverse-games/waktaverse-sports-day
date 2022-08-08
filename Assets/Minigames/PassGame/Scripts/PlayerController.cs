using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class PlayerController : MonoBehaviour
    {
        // reference
        // https://www.youtube.com/watch?v=7Vgb8MoWIq8
    
        bool isJump = false;
        bool isTop = false;
        public float jumpHeight = 0;
        public float jumpSpeed = 0;

        private Vector2 startPosition;
        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJump = true;
            }
            else if (transform.position.y <= startPosition.y)
            {
                isJump = false;
                isTop = false;
                transform.position = startPosition;
            }

            if (isJump)
            {
                if (transform.position.y <= jumpHeight - 0.1f && !isTop)
                {
                    transform.position = Vector2.Lerp(transform.position,
                        new Vector2(transform.position.x, jumpHeight), jumpSpeed * Time.deltaTime);
                }
                else
                {
                    isTop = true;
                }

                if (transform.position.y > startPosition.y && isTop)
                {
                    transform.position = Vector2.MoveTowards(transform.position, startPosition, jumpSpeed * Time.deltaTime);
                }
            }
        }
    }
}