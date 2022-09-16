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
        private float initialSpeed = 3f;
        private float speed;                // �÷��� �ӵ�
        [SerializeField]
        private float jumpAmount = 100f;

        public Vector3 force;
        private Vector2 direction, position;

        [SerializeField]
        private Transform wallLeft, wallRight;
        private float clampLeft, clampRight;

        private bool isFired;               // ���� �߻�Ǿ����� ����
        private bool isJumping;             // ������?

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float InitialSpeed
        {
            get { return initialSpeed; }
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
                        GameManager.Instance.BallFire();
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
                // A, DŰ�� ����� �¿� �̵�
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
            if (collision.collider.CompareTag("Coin"))
            {
                GameManager.Instance.Money += collision.collider.GetComponent<Coin>().CoinValue;
                Destroy(collision.gameObject);
            }
            if (collision.collider.CompareTag("CrashGame_Item"))
            {
                // ������ ��� ���
                collision.collider.GetComponent<Item>().ActivateItem();
                Destroy(collision.gameObject);
            }
        }

        public void PlatformInit()
        {
            Speed = initialSpeed;
            BallInit();
        }

        private void BallInit()
        {
            ball = Ball.SpawnBall(ballStartPosition.position);
            BallInit(ball);
        }

        private void BallInit(Ball ball)
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

