using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GameHeaven.PassGame
{
    public class PlayerController : MonoBehaviour
    {
        // reference : https://www.youtube.com/watch?v=qWWYLdnoZkI&list=PLbkWuDuFAzKVjC0FxUHAJKn5Vxc3GrymV&index=2
        // reference : https://www.youtube.com/watch?v=RmqWuoFHD5g
    
        public float JumpForce = 6.0f;
        public Text ScoreText;
        public MonsterResponseManager MonsterResponseManager;
        
        private LayerMask _groundLayer;
        private LayerMask _MonsterHeadLayer;
        private LayerMask _MonsterBodyLayer;
        private Rigidbody2D _rb2d;
        private BoxCollider2D _boxCollider2D;
        private int _jumpCount = 0;
        private int _score = 0;
        private Vector2 _footPosistion;
        private bool _isSpaceUp = true;
        private GameObject _monster;
        private const int MaxJumpCount = 2;
        private void Start()
        {
            _rb2d = GetComponent<Rigidbody2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _groundLayer = LayerMask.GetMask("Ground");
            _MonsterHeadLayer = LayerMask.GetMask("MonsterHead");
            _MonsterBodyLayer = LayerMask.GetMask("MonsterBody");

            _monster = MonsterResponseManager.GetNextMonster();
        }

        private void Update()
        {
            UpdateJumpState();
            UpdatePlayerFootState();
            CalMonsterPass();
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
        }

        // Unity Editor ���� �׸���
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_footPosistion, 0.01f);
        }

        void CalMonsterPass()
        {
            Debug.Log(_monster.transform.position.x - this.transform.position.x);
            if (_monster.transform.position.x + 2 < this.transform.position.x)
            {
                var monsterBase = _monster.GetComponent<MonsterBase>();
                if (monsterBase.IsAlive())
                {
                    AddScore(monsterBase.GetScore());
                }
                _monster = MonsterResponseManager.GetNextMonster();
            }
        }
        
        // �����ϱ�
        void SetJump()
        {
            _rb2d.velocity = Vector2.up * JumpForce;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((1 << collision.collider.gameObject.layer) == _MonsterHeadLayer.value)
            {
                SetJump();
                var monsterBase = collision.gameObject.GetComponent<MonsterBase>();
                monsterBase.StompMonster();
                AddScore(monsterBase.GetScore());
            }
            else if ((1 << collision.collider.gameObject.layer) == _MonsterBodyLayer.value)
            {
                Debug.Log("End");
                // will be fixed
                Application.Quit();
            }
        }

        private void AddScore(int score)
        {
            _score += score;
            ScoreText.text = $"Score : {_score}";
        }
    }
}