using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class Player : MonoBehaviour
    {
        public float speed = 1.0f;
        
        private bool _isFront = true;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _screenBoundaries;
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
        }

        // Update is called once per frame
        void Update()
        {
            ChangeDirection();
            MovePlayer();
        }

        private void ChangeDirection()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_isFront)
                {
                    _spriteRenderer.flipX = false;
                    _isFront = false;
                }
                else
                {
                    _spriteRenderer.flipX = true;
                    _isFront = true;
                }
                speed *= -1;
            }
        }

        private void MovePlayer()
        {
            Vector3 pos = UnityEngine.Camera.main.WorldToViewportPoint(transform.position);
            if (pos.x < 0f)
            {
                pos.x = 0f;
                transform.position = UnityEngine.Camera.main.ViewportToWorldPoint(pos);
            }
            else if (pos.x > 1f)
            {
                pos.x = 1f;
                transform.position = UnityEngine.Camera.main.ViewportToWorldPoint(pos);
            }
            else
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
        }
    }
}

