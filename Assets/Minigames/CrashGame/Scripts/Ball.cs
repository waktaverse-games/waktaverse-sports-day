using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class Ball : MonoBehaviour
    {
        const float Radian = 180f;

        public Rigidbody2D rigidBody;
        [SerializeField]
        private float speed;

        private Vector2 position;
        private Vector2 bounceDirection;
        private Vector3 force;

        // Start is called before the first frame update
        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            
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
            rigidBody.AddForce(force);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Platform"))
            {
                Debug.Log($"Current Ball Speed: {rigidBody.velocity.magnitude}");
            }
            else if (collision.collider.CompareTag("Brick"))
            {
                collision.gameObject.GetComponent<Brick>().BallCollide();
            }
            else if (collision.collider.CompareTag("Bottom"))
            {
                GameManager.Instance.GameOver();
            }
        }
    }

}
