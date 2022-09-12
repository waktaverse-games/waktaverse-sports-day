using System;
using System.Collections;
using System.Collections.Generic;
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
        void Start()
        {
            _name = gameObject.name;
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
            if (_name == "pyochang")
            {
                transform.Rotate(0, 0, 180 * Time.deltaTime * _rotateDir);
            }
        }
    }
}

