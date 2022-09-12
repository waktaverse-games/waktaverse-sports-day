using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class Projectile : MonoBehaviour
    {
        public GameManager gameManager;
        public int damage;

        private string _name;
        private float _rotateDir;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        void OnEnable()
        {
            _name = gameObject.name;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetState(bool toRight)
        {
            if (toRight)
            {
                _rotateDir = -1f;
                _spriteRenderer.flipX = false;
            }
            else
            {
                _rotateDir = 1f;
                _spriteRenderer.flipX = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_name == "pyochang(Clone)")
            {
                transform.Rotate(0, 0, 180 * Time.deltaTime * _rotateDir);
            }

            if (transform.position.y <= 0f)
            {
                gameObject.SetActive(false);
            }
        }

        public void PoopThrow()
        {
            Vector3 currentPos = transform.position;
            Vector3 newPos = new Vector3(currentPos.x - 3, 0f, currentPos.z);
            _rigidbody.DOJump(newPos, 2, 1, 1);
        }

        public void ShootArrow(bool isTowardRight, float speed)
        {
            if (!isTowardRight) speed *= -1;
            Vector3 currentPos = transform.position;
            Vector3 newPos = new Vector3(currentPos.x + speed, 0f, currentPos.z);
        }
    }
}

