using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        float speed;
        [SerializeField]
        float slowDownSpeed;

        private bool movingLeft;

        Rigidbody2D rb;
        SpriteRenderer sprite;

        private Vector2 dir = Vector2.right;
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            ChangeDirection();
            FlipSprite();
        }
        void FixedUpdate()
        {
            rb.velocity += dir * speed;
        }

        private void ChangeDirection()
        {
            if (Input.GetKeyDown(KeyCode.A) && !movingLeft)
            {
                movingLeft = true;
                rb.velocity = dir * slowDownSpeed;
                dir = Vector2.left;
            }
            if (Input.GetKeyDown(KeyCode.D) && movingLeft)
            {
                movingLeft = false;
                rb.velocity = dir * slowDownSpeed;
                dir = Vector2.right;
            }
        }
        private void FlipSprite()
        {
            if(rb.velocity.x >= 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "GameOverArea")
            {
                GameManager.instance.isGameOver = true;
            }
        }
    }
}
