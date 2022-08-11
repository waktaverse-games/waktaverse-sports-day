using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class PlayerController : MonoBehaviour
    {
        // reference : https://www.youtube.com/watch?v=qWWYLdnoZkI&list=PLbkWuDuFAzKVjC0FxUHAJKn5Vxc3GrymV&index=2
        // reference : https://www.youtube.com/watch?v=RmqWuoFHD5g
    
        public float JumpForce = 6.0f;

        private LayerMask _groundLayer;
        private LayerMask _MonsterHeadLayer;
        private LayerMask _MonsterBodyLayer;
        private Rigidbody2D _rb2d;
        private BoxCollider2D _boxCollider2D;
        private int _jumpCount = 0;
        private Vector2 _footPosistion;
        private bool _isSpaceUp = true;
        
        private const int MaxJumpCount = 2;
        private void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _groundLayer = LayerMask.GetMask("Ground");
            _MonsterHeadLayer = LayerMask.GetMask("MonsterHead");
            _MonsterBodyLayer = LayerMask.GetMask("MonsterBody");
        }

        private void Update()
        {
            UpdateJumpState();
            UpdatePlayerFootState();
        }
        
        // ���� ���� üũ
        void UpdateJumpState()
        {
            if (_isSpaceUp && Input.GetKeyDown(KeyCode.Space) && _jumpCount < MaxJumpCount)
            {
                _isSpaceUp = false;
                _jumpCount++;
                SetJump();
            }
            else if (!_isSpaceUp && Input.GetKeyUp(KeyCode.Space))
            {
                _isSpaceUp = true;
            }
        }

        // Player�� �߹ٴ� üũ
        void UpdatePlayerFootState()
        {
            Bounds bounds = _boxCollider2D.bounds;
            _footPosistion = new Vector2(bounds.center.x, bounds.min.y);
            if (_isSpaceUp && Physics2D.OverlapCircle(_footPosistion, 0.01f, _groundLayer))
            {
                _jumpCount = 0;
            }
            else if (_isSpaceUp && Physics2D.OverlapCircle(_footPosistion, 0.01f, _MonsterHeadLayer))
            {
                SetJump();
            }
        }

        // Unity Editor ���� �׸���
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_footPosistion, 0.01f);
        }

        // �����ϱ�
        void SetJump()
        {
            _rb2d.velocity = Vector2.up * JumpForce;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((1 << collision.collider.gameObject.layer) == _MonsterBodyLayer.value)
            {
                Debug.Log("End");
                // will be fixed
                Application.Quit();
            }
        }
    }
}