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
        private const int maxBallNumber = 18;

        private bool isFired;
        private int stuckTimeout;

        public Rigidbody2D rigidBody;
        [SerializeField]
        private float initialForce = 300;
        [SerializeField]
        private float maxSpeed = 8f;

        private PlayerPlatform platform;

        [SerializeField]
        private bool randomBounce;

        public bool isReturning;

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
                //Debug.Log($"ballNumber = {ballNumber}");
            }
        }

        // Start is called before the first frame update
        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            isReturning = false;
            isFired = false;
            platform = MiniGameManager.Instance.platform;
        }


        private void OnEnable()
        {
            stuckTimeout = 0;
            isFired = false;
            isReturning = false;
        }

        private void Update()
        {
            if (isFired)
            {
                if (rigidBody.velocity.sqrMagnitude < 0.01f)
                {
                    rigidBody.velocity = rigidBody.velocity.normalized * 4;
                    if (rigidBody.velocity.sqrMagnitude < 1f) stuckTimeout++;
                }
                if (stuckTimeout > 120)
                {
                    stuckTimeout = 0;
                    DestroyBall();
                }
            }
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (MiniGameManager.Instance.CurrentGameState == Utils.GameState.Over)
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
            if ((Mathf.Abs(rigidBody.velocity.y) < 0.05f) || Mathf.Abs(rigidBody.velocity.x) < 0.05f)
            {
                // 튕겨나가는 각도가 너무 작을 때 보정
                rigidBody.velocity = Utils.RotateVector(rigidBody.velocity, 10f);
            }
            isReturning = true;
            MiniGameManager.Instance.Sound.PlayEffect("button_01", volume: .5f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Vector2 velocity = rigidBody.velocity;
            if (collision.CompareTag("Platform"))
            {
                // 공이 여러번 플랫폼에 튕기는 행위 방지
                if (isReturning)
                {
                    if (randomBounce)
                    {
                        velocity = Utils.RotateVector(velocity, (Random.value - 0.5f) * 30f);
                    }
                    else
                    {
                        // 플랫폼 운동방향에 따라 반사 각도 변화
                        Vector2 platformVelocity = collision.GetComponentInParent<Rigidbody2D>().velocity;

                        float delta = Mathf.Sign(velocity.y) * ((-platformVelocity.x * 2f) + (platformVelocity.y * Mathf.Sign(velocity.x) * 1.3f));
                        velocity = Utils.RotateVector(velocity, delta);
                    }
                    if (Mathf.Abs(velocity.y) < 0.05f)
                    {
                        // 튕겨나가는 각도가 너무 작을 때 보정
                        velocity = Utils.RotateVector(velocity, 10f);
                    }
                    // 점프 도중 플랫폼과 공의 충돌 시 공이 아래로 튕기는 것을 방지. 공은 무조건 위로 튕김
                    if (rigidBody.velocity.y < 0) velocity = new Vector2(velocity.x, -velocity.y);
                    //Debug.Log(Vector2.Angle(velocity, Vector2.right));
                    isReturning = false;

                    // 공에 닿을 시 점프 중단.
                    platform.Stop();
                    MiniGameManager.Instance.Sound.PlayEffect("button_01", pitch: .5f, volume: .5f);
                }
            }
            rigidBody.velocity = velocity;
            
        }

        public static Ball SpawnBall(Vector2 position)
        {
            if (BallNumber < maxBallNumber)
            {
                BallNumber++;
                Ball ball = MiniGameManager.ObjectPool.GetObject("Ball").GetComponent<Ball>();
                ball.transform.position = position;
                ball.transform.SetParent(MiniGameManager.Instance.Item.BallParent, true);
                return ball;
            }
            else return null;
        }

        public void DestroyBall()
        {
            //Debug.Log("DestroyBall called");
            BallNumber--;
            if (BallNumber <= 0) MiniGameManager.Instance.GameOver();
            MiniGameManager.ObjectPool.ReturnObject("Ball", gameObject);
        }

        public void StopBall()
        {
            rigidBody.velocity = Vector2.zero;
        }

        public void ResetBallSpeed()
        {
            // 공의 속도를 최초 속도로 초기화
            Vector2 tempVelocity = rigidBody.velocity;
            rigidBody.velocity = Vector2.zero;
            rigidBody.AddForce(tempVelocity.normalized * InitialForce);
        }

        public void Fire(Vector2 force)
        {
            isFired = true;
            MiniGameManager.Instance.Sound.PlayEffect("button_01", volume: .5f);
            rigidBody.AddForce(force);
        }

        public void Fire()
        {
            Fire(new Vector2(Random.Range(-10, 10), Random.Range(5, 10)).normalized * InitialForce);
            isReturning = false;
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
            //Debug.Log($"Ball Speed: {rigidBody.velocity.magnitude}");
        }
    }

}
