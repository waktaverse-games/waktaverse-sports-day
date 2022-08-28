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

        private static int ballNumber = 0;

        public Rigidbody2D rigidBody;
        [SerializeField]
        private float initialForce = 300;
        [SerializeField]
        private float maxSpeed = 8f;

        private PlayerPlatform platform;
        private Vector2 velocity;

        [SerializeField]
        private bool randomBounce;
        private bool isReturning;

        public float InitialForce
        {
            get { return initialForce; }
        }

        public static int BallNumber
        {
            get { return ballNumber; }
            set 
            {
                ballNumber = value;
                Debug.Log($"ballNumber = {ballNumber}");
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            isReturning = false;
            platform = GameManager.Instance.platform;
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
                if (rigidBody.velocity.sqrMagnitude < (maxSpeed * maxSpeed)) AddSpeed(speedGrowth);
            }
            else if (collision.collider.CompareTag("Bottom"))
            {
                DestroyBall();
            }
            isReturning = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            velocity = rigidBody.velocity;
            if (collision.CompareTag("Platform"))
            {
                // 플랫폼의 운동상태에 상관없이, 플랫폼에 닿으면 일정 각도로 반사
                // 공이 여러번 플랫폼에 튕기는 행위 방지
                if (isReturning)
                {
                    // 점프 도중 플랫폼과 공의 충돌 시 공이 아래로 튕기는 것을 방지. 공은 무조건 위로 튕김
                    if (randomBounce)
                    {
                        velocity = Utils.RotateVector(velocity, (Random.value - 0.5f) * 30f);
                    }
                    else
                    {
                        Vector2 platformVelocity = collision.GetComponent<Rigidbody2D>().velocity;
                        Debug.Log($"Platform Velocity: {platformVelocity.x}, {platformVelocity.y}");

                        float delta = Mathf.Sign(velocity.y) * ((-platformVelocity.x * 2f) + (platformVelocity.y * Mathf.Sign(velocity.x) * 1.3f));
                        velocity = Utils.RotateVector(velocity, delta);
                    }
                    if (Mathf.Abs(velocity.y) < 0.05f)
                    {
                        // 튕겨나가는 각도가 너무 작을 때 보정
                        velocity = Utils.RotateVector(velocity, 10f);
                    }
                    if (rigidBody.velocity.y < 0) velocity = new Vector2(velocity.x, -velocity.y);
                    Debug.Log(Vector2.Angle(velocity, Vector2.right));
                    isReturning = false;

                    // 공에 닿을 시 점프 중단.
                    platform.Stop();
                }
            }
            rigidBody.velocity = velocity;
        }

        public static Ball SpawnBall(Vector2 position)
        {
            BallNumber++;
            Ball ball = Instantiate(GameManager.Instance.ballPrefab, position, Quaternion.identity);
            ball.transform.SetParent(GameManager.Instance.Item.ItemParent, true);
            return ball;
        }

        public void DestroyBall()
        {
            Debug.Log("DestroyBall called");
            BallNumber--;
            if (BallNumber <= 0) GameManager.Instance.GameOver();
            Destroy(gameObject);
        }

        public void StopBall()
        {
            rigidBody.velocity = Vector2.zero;
        }

        public void Fire(Vector2 force)
        {
            isReturning = false;
            rigidBody.AddForce(force);
        }

        public void Fire()
        {
            isReturning = false;
            rigidBody.AddForce(new Vector2(1, 1).normalized * InitialForce);
        }

        public void BlockFire()
        {
            Vector2 fireForce = new Vector2((Random.Range(0, 2) - 0.5f) * 2, -1).normalized * InitialForce;
            Fire(fireForce);
            isReturning = true;
        }

        private void AddSpeed(float growth)
        {
            rigidBody.velocity *= (1 + growth);
            Debug.Log($"Ball Speed: {rigidBody.velocity.magnitude}");
        }
    }

}
