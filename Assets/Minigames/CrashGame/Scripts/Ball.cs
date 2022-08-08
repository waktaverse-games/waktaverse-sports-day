using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class Ball : MonoBehaviour
    {
        const float Radian = 180f;

        private Rigidbody2D rigidBody;
        [SerializeField]
        private float speed;

        private Vector3 force;

        bool isComing;

        // Start is called before the first frame update
        void Awake()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            force = new Vector3(speed, speed, 0);
        }

        private void Start()
        {
            //Vector3 tmp = transform.eulerAngles;
            //tmp.z += 30;
            //transform.eulerAngles = tmp;
            rigidBody.AddForce(force);
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            //position = rigidBody.position;
            //rigidBody.MovePosition(position + (Vector2)transform.up * speed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Platform"))
            {
                Debug.Log($"Current Ball Speed: {rigidBody.velocity.magnitude}");
                //isComing = false;
                //bounceDirection = rigidBody.velocity;
                //rigidBody.velocity = Vector2.zero;

                //bounceDirection = (new Vector2(bounceDirection.x, -bounceDirection.y)).normalized * speed;
                //Debug.Log("Bounced");
                //rigidBody.AddForce(bounceDirection);
            }
            else if (collision.collider.CompareTag("Brick"))
            {
                collision.gameObject.GetComponent<Brick>().BallCollide();
            }
            else
            {

            }
        }
    }

}
