using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class Ball : MonoBehaviour
    {
        private static int radian = 180;
        private static float speedGrowth = 0.05f;
        private static float speedCap;

        public Rigidbody2D rigidBody;
        [SerializeField]
        private float speed;
        [SerializeField]
        private PlayerPlatform platform;
        private Vector2 velocity;

        [SerializeField]
        private bool randomBounce;
        private bool isReturning;


        public float InitialSpeed
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
            if (GameManager.Instance.CurrentGameState == Utils.GameState.Over)
            {
                StopBall();
                
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Brick"))
            {
                collision.gameObject.GetComponent<Brick>().BallCollide();
                AddSpeed(speedGrowth);
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
                    velocity = rigidBody.velocity;
                    if (randomBounce)
                    {
                        velocity = Utils.RotateVector(velocity, (Random.value - 0.5f) * 30f);
                    }
                    if (Mathf.Abs(velocity.y) < 0.05f)
                    {
                        // 튕겨나가는 각도가 너무 작을 때 보정
                        velocity = Utils.RotateVector(velocity, 10f);
                    }
                    if (rigidBody.velocity.y < 0) velocity = new Vector2(velocity.x, -velocity.y);
                    Debug.Log(Vector2.Angle(velocity, Vector2.right));
                    rigidBody.velocity = velocity;
                    isReturning = false;

                    // 공에 닿을 시 점프 중단.
                    platform.Stop();
                }
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

        private void AddSpeed(float growth)
        {
            rigidBody.velocity *= (1 + growth);
            Debug.Log(rigidBody.velocity.magnitude);
        }
    }

}
