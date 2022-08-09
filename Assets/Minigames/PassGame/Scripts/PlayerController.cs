using System;
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
        public float AddJumpHeight = 2;
        public float JumpSpeed = 7;

        private Vector2 _startPosition;
        private int _jumpCount = 0;
        private float _maxJumpHeight = 0;
        private bool _isSpaceUp = true;
        
        private const int MaxJumpCount = 2;

        void Start()
        {
            _startPosition = transform.position;
        }

        void Update()
        {
            UpdateJumpState();

            if (_isJump)
            {
                if (_isTop)
                {
                    UpdateFallDown();
                }
                else
                {
                    UpdateJump();
                }
            }
        }

        void UpdateJumpState()
        {
            if (_isSpaceUp && Input.GetKeyDown(KeyCode.Space) && _jumpCount < MaxJumpCount)
            {
                _jumpCount++;
                _isSpaceUp = false;
                
                SetJump();
            }
            else if (transform.position.y <= _startPosition.y)
            {
                InitJump();
            }

            if (!_isSpaceUp && Input.GetKeyUp(KeyCode.Space))
            {
                _isSpaceUp = true;
            }
        }

        void SetJump()
        {
            _isJump = true;
            _isTop = false;
            _maxJumpHeight = transform.position.y + AddJumpHeight;
        }

        void InitJump()
        {
            _isJump = false;
            _isTop = false;
            _jumpCount = 0;
            transform.position = _startPosition;
        }
        void UpdateJump()
        {
            if (transform.position.y <= _maxJumpHeight - 0.1f)
            {
                transform.position = Vector2.Lerp(transform.position,
                    new Vector2(transform.position.x, _maxJumpHeight), JumpSpeed * Time.deltaTime);
            }
            else
            {
                _isTop = true;
            }
        }
        
        void UpdateFallDown()
        {
            if (transform.position.y > _startPosition.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, _startPosition, JumpSpeed * Time.deltaTime);
            }
        }
    }
}