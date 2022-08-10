using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class Ball : MonoBehaviour
    {
        private static int radian = 180;

        public Rigidbody2D rigidBody;
        [SerializeField]
        private float speed;
        [SerializeField]
        private PlayerPlatform platform;
        private Vector2 direction;

        [SerializeField]
        private bool isReturning;

        public float BallSpeed
        {
            get { return speed; }
        }

        // Start is called before the first frame update
        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            isReturning = false;
        }

        private void Start()
        {
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (GameManager.Instance.CurrentGameState == Define.GameState.Over)
            {
                StopBall();
                
            }
        }

        public void StopBall()
        {
            rigidBody.velocity = Vector2.zero;
        }

        public void Fire(Vector3 force)
        {
            isReturning = false;
            rigidBody.AddForce(force);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Brick"))
            {
                collision.gameObject.GetComponent<Brick>().BallCollide();
            }
            else if (collision.collider.CompareTag("Bottom"))
            {
                GameManager.Instance.GameOver();
            }
            isReturning = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Platform"))
            {
                // 플랫폼의 운동상태에 상관없이, 플랫폼에 닿으면 일정 각도로 반사
                // 공이 여러번 플랫폼에 튕기는 행위 방지
                if (isReturning)
                {
                    // 점프 도중 플랫폼과 공의 충돌 시 공이 아래로 튕기는 것을 방지. 공은 무조건 위로 튕김 
                    if (rigidBody.velocity.y < 0) direction = new Vector2(rigidBody.velocity.x, -rigidBody.velocity.y).normalized;
                    else direction = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y).normalized;
                    rigidBody.velocity = Vector2.zero;
                    rigidBody.AddForce(direction * 200);
                    isReturning = false;

                    // 공에 닿을 시 점프 중단.
                    platform.Stop();
                }
            }
        }
    }

}
