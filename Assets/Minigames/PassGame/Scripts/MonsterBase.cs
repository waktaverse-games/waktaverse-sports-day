using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class MonsterBase : MonoBehaviour
    {
        public float MoveSpeed = 0f;
        public Vector2 StartPosition;
        private int _score = 10;
        private bool _isAlive = true;
        
        private void OnEnable()
        {
            _isAlive = true;
            _score = 10;
            transform.position = StartPosition;
        }

        void Update()
        {
            if (_isAlive)
            {
                MoveLeft();
            }
            else
            {
                DropDown();
            }
        }

        private void MoveLeft()
        {
            transform.Translate(Vector2.left * Time.deltaTime * MoveSpeed);

            if (transform.position.x < -10f)
            {
                gameObject.SetActive(false);
            }
        }

        private void DropDown()
        {
            var left = Vector2.left * MoveSpeed / 2;
            var down = Vector2.down * MoveSpeed;
            
            transform.Translate((left + down) * Time.deltaTime);

            if (transform.position.y < -10f)
            {
                gameObject.SetActive(false);
            }
        }

        public void StompMonster()
        {
            _isAlive = false;
            _score = 25;
        }

        public int GetScore()
        {
            return _score;
        }

        public bool IsAlive()
        {
            return _isAlive;
        }
    }
}
