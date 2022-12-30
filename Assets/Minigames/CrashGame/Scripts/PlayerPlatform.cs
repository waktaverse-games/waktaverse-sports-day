using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharedLibs;
using SharedLibs.Character;
using TMPro;

namespace GameHeaven.CrashGame
{
    public class PlayerPlatform : MonoBehaviour
    {

        private Ball ball;

        private Rigidbody2D rigidBody;
        private float horizontal;
        [SerializeField]
        private float initialSpeed = 8f;
        [SerializeField]
        private float initialPlatformXScale = 1.5f;
        private float speed;                // 플랫폼 속도
        [SerializeField]
        private float jumpAmount = 100f;

        public Vector3 force;
        private Vector2 direction, position;

        [SerializeField]
        private Transform wallLeft, wallRight;
        private float clampLeft, clampRight;

        private bool isFired;               // 공이 발사되었는지 여부
        private bool isJumping;             // 점프중?

        private Transform ballStartPosition;
        private Transform platform;
        private Transform playerCharacter;

        private SpriteRenderer playerSprite;

        private Animator playerAnimator;

        [SerializeField]
        private List<Sprite> platformSprite;

        public Transform BallStartPosition { get { return ballStartPosition; } }
        public Transform Platform { get { return platform; } }
        public Transform PlayerCharacter { get { return playerCharacter; } }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float InitialPlatformXScale { get { return initialPlatformXScale; } }
        public float InitialSpeed { get { return initialSpeed; } }

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            clampLeft = wallLeft.transform.position.x + 1;
            clampRight = wallRight.transform.position.x - 1;
            ballStartPosition = transform.GetChild(0);
            playerCharacter = transform.GetChild(1);
            playerSprite = playerCharacter.GetComponentInChildren<SpriteRenderer>();
            platform = transform.GetChild(2);
            playerAnimator = GetComponentInChildren<Animator>();
            //force = new Vector3(1, 1, 0).normalized * ball.InitialSpeed;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (MiniGameManager.Instance.CurrentGameState == Utils.GameState.Start)
            {
                if (!isFired)
                {
                    ball.transform.position = ballStartPosition.position;
                    if (Input.GetKey(KeyCode.Space))
                    {
                        isFired = true;
                        MiniGameManager.Instance.BallFire();
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
            if (MiniGameManager.Instance.CurrentGameState == Utils.GameState.Start)
            {
                // A, D키를 사용해 좌우 이동
                horizontal = Input.GetAxis("Horizontal");
                rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
                playerAnimator.SetBool("Move", Math.Abs(horizontal) > 0.01);
                if (horizontal > 0) playerSprite.flipX = true;
                else if (horizontal < 0) playerSprite.flipX = false;
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
                CrashGame.MiniGameManager.Instance.Sound.PlayEffect("coin_01");
                MiniGameManager.Instance.AddScore(collision.collider.GetComponent<Coin>().CoinValue);
                collision.collider.GetComponent<Coin>().ShowEffect();
                MiniGameManager.ObjectPool.ReturnObject("coin", collision.collider.gameObject);
            }
            if (collision.collider.CompareTag("CrashGame_Item"))
            {
                MiniGameManager.Instance.Sound.PlayEffect("coin_02");
                // 아이템 즉발 사용
                collision.collider.GetComponent<Item>().ActivateItem();
                collision.collider.GetComponent<Item>().DestroyItem();
            }
        }

        //private void OnCollisionStay2D(Collision2D collision)
        //{
        //    if (collision.collider.CompareTag("Bottom"))
        //    {
        //        Land();
        //    }
        //}

        public void OnGameOver()
        {
            playerAnimator.SetBool("GameOver", true);
            rigidBody.velocity = Vector2.zero;
        }

        public void PlatformInit()
        {
            platform.localScale = new Vector3(initialPlatformXScale, 2, 1);
            Speed = initialSpeed;
            rigidBody.position = MiniGameManager.Instance.playerSpawnPosition.position;
            platform.GetComponent<SpriteRenderer>().sprite = platformSprite[UnityEngine.Random.Range(0, platformSprite.Count)];
            playerAnimator.SetBool("GameOver", false);
            BallInit();
        }

        public void SetCharacter(CharacterType currentCharacter)
        {
            playerSprite.sprite = MiniGameManager.Instance.PlayerSpriteList[(int)currentCharacter];
            playerSprite.sortingOrder = 1;
            playerAnimator.runtimeAnimatorController = MiniGameManager.Instance.PlayerAnimatorControllerList[(int)currentCharacter];
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
                playerAnimator.SetBool("Jump", true);
            }
        }

    private void Land() 
        {
            isJumping = false;
            playerAnimator.SetBool("Jump", false);
        }

        public void Stop()
        {
            rigidBody.velocity = Vector2.zero;
            playerAnimator.SetBool("Jump", false);
            playerAnimator.SetBool("Move", false);
        }
    }
}

