using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class Coin : MonoBehaviour
    {
        public float deletePosX;
        public float speed = 5.0f;
        
        private Rigidbody2D _rigidbody;
        
        void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.velocity = new Vector2(-speed, 0);
        }

        private void Update()
        {
            if (transform.position.x < deletePosX)
            {
                gameObject.SetActive(false);
            }
        }
    }
}