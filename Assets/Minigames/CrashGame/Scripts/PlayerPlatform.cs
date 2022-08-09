using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class PlayerPlatform : MonoBehaviour
    {
        public Ball ball;

        private Transform ballStartPosition;
        private Rigidbody2D rigidBody;
        private float horizontal;
        [SerializeField]
        private float speed = 3f;           // 플랫폼 속도
        [SerializeField]
        private float ballSpeed = 200f;     // 공 속도

        private Vector3 force;
        private Vector2 direction, position;

        [SerializeField]
        private Transform wallLeft, wallRight;
        private float clampLeft, clampRight;

        private bool isFired;               // 공이 발사되었는지 여부

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
            ballStartPosition = transform.GetChild(0);
            force = new Vector3(ballSpeed, ballSpeed, 0);
        }

        private void Start()
        {
            BallInit();
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameState == Define.GameState.Start)
            {
                if (!isFired)
                {
                    ball.transform.position = ballStartPosition.position;
                    if (Input.GetKey(KeyCode.Space))
                    {
                        isFired = true;
                        ball.Fire(force);
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentGameState == Define.GameState.Start)
            {
                // A, D키를 사용해 좌우 이동
                horizontal = Input.GetAxis("Horizontal");
                direction = new Vector2(horizontal, 0);
                position = new Vector2(Math.Clamp(rigidBody.position.x, clampLeft, clampRight), rigidBody.position.y);
                rigidBody.MovePosition(position + direction * speed * Time.deltaTime);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

        }

        public void BallInit()
        {
            ball.StopBall();
            ball.transform.position = ballStartPosition.position;
            isFired = false;
        }
    }
}

