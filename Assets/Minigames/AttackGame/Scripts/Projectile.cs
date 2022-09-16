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
        public int tweenId;
        public bool isHit;

        private string _name;
        private float _rotateDir;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        private Tween _tween;
        void OnEnable()
        {
            _name = gameObject.name;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            isHit = false;
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
                transform.Rotate(0, 0, 20 * Time.deltaTime * _rotateDir);
            }

            if (transform.position.y <= 0f)
            {
                StopAllCoroutines();
                _tween.Kill();
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Outline"))
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _tween.Kill();
            DOTween.Kill(tweenId);
        }

        public void PoopThrow(float distance)
        {
            Vector3 currentPos = transform.position;
            Vector3 newPos = new Vector3(currentPos.x - distance, 0f, currentPos.z);
            _tween = _rigidbody.DOJump(newPos, 2, 1, 1).SetId(tweenId);
        }

        public void ShootArrow(bool isTowardRight, float speed)
        {
            float tempDir = 1f;
            if (!isTowardRight) tempDir = -1;
            Vector3 currentPos = transform.position;
            _tween = transform.DOMoveX(currentPos.x + 20 * tempDir, 10-speed).SetId(tweenId);
        }

        public void ShootPyochang(bool isTowardRight, float speed, float distance)
        {
            float tempDir = 1f;
            if (!isTowardRight) tempDir = -1;
            Vector3 currentPos = transform.position;
            _tween = transform.DOJump(new Vector3(currentPos.x + distance * tempDir, 0f, currentPos.z), 
                1.5f, 1, speed).SetId(tweenId);
        }
    }
}

