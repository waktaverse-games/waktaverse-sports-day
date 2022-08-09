using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class PlayerPlatform : MonoBehaviour
    {
        private Rigidbody2D rigidBody;
        private float horizontal;
        [SerializeField]
        private float speed = 3f;           // ÇÃ·§Æû ¼Óµµ

        private Vector2 direction, position;

        [SerializeField]
        private Transform wallLeft, wallRight;
        private float clampLeft, clampRight;

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            clampLeft = wallLeft.transform.position.x + 1;
            clampRight = wallRight.transform.position.x - 1;
        }

        private void Start()
        {


        }

        private void FixedUpdate()
        {
            // A, DÅ°¸¦ »ç¿ëÇØ ÁÂ¿ì ÀÌµ¿
            horizontal = Input.GetAxis("Horizontal");
            direction = new Vector2(horizontal, 0);
            position = new Vector2(Math.Clamp(rigidBody.position.x, clampLeft, clampRight), rigidBody.position.y);
            rigidBody.MovePosition(position + direction * speed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

        }
    }
}

