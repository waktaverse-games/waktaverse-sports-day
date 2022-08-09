using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class PlayerController : MonoBehaviour
    {
        // reference
        // https://www.youtube.com/watch?v=7Vgb8MoWIq8
    
        bool _isJump = false;
        bool _isTop = false;
        public float JumpHeight = 0;
        public float JumpSpeed = 0;

        private Vector2 startPosition;
        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isJump = true;
            }
            else if (transform.position.y <= startPosition.y)
            {
                _isJump = false;
                _isTop = false;
                transform.position = startPosition;
            }

            if (_isJump)
            {
                if (transform.position.y <= JumpHeight - 0.1f && !_isTop)
                {
                    transform.position = Vector2.Lerp(transform.position,
                        new Vector2(transform.position.x, JumpHeight), JumpSpeed * Time.deltaTime);
                }
                else
                {
                    _isTop = true;
                }

                if (transform.position.y > startPosition.y && _isTop)
                {
                    transform.position = Vector2.MoveTowards(transform.position, startPosition, JumpSpeed * Time.deltaTime);
                }
            }
        }
    }
}