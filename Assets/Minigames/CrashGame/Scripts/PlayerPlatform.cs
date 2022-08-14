using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class PlayerPlatform : MonoBehaviour
    {
        private Ball ball;

        private Transform ballStartPosition;
        private Rigidbody2D rigidBody;
        private float horizontal;
        [SerializeField]
        private float speed = 3f;           // 플랫폼 속도
        [SerializeField]
        private float jumpAmount = 100f;

        public Vector3 force;
        private Vector2 direction, position;

        [SerializeField]
        private Transform wallLeft, wallRight;
        private float clampLeft, clampRight;

        private bool isFired;               // 공이 발사되었는지 여부
        private bool isJumping;             // 점프중?

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
            //force = new Vector3(1, 1, 0).normalized * ball.InitialSpeed;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameState == Utils.GameState.Start)
            {
                if (!isFired)
                {
                    ball.transform.position = ballStartPosition.position;
                    if (Input.GetKey(KeyCode.Space))
                    {
                        isFired = true;
                        ball.Fire();
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Jump();
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentGameState == Utils.GameState.Start)
            {
                // A, D키를 사용해 좌우 이동
                horizontal = Input.GetAxis("Horizontal");
                rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Bottom"))
            {
                Land();
            }
            //else if (collision.collider.CompareTag("Ball"))
            //{
                
            //}
        }

        public void BallInit()
        {
            ball = Ball.SpawnBall(ballStartPosition.position);
            BallInit(ball);
        }

        public void BallInit(Ball ball)
        {
            ball.StopBall();
            isFired = false;
        }

        private void Jump()
        {
            if (!isJumping)
            {
                rigidBody.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
                isJumping = true;
            }
        }

        private void Land() { isJumping = false; }

        public void Stop()
        {
            rigidBody.velocity = Vector2.zero;
        }
    }
}

