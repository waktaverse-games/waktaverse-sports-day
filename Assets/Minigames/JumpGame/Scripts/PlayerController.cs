using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class PlayerController : MonoBehaviour
    {
        public float maxSpeed;
        Rigidbody2D rb;
        SpriteRenderer sprite;
        Animator animator;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if(Input.GetButtonUp("Horizontal"))
            {
                rb.velocity = new Vector2(rb.velocity.normalized.x * 0.5f, rb.velocity.y);
            }

            UpdateFace();
        }
        private void FixedUpdate()
        {
            PlayerMovement();
        }

        void UpdateFace()
        {
            if (rb.velocity.x > 0)
                sprite.flipX = true;
            else
                sprite.flipX = false;
        }

        void PlayerMovement()
        {
            float h = Input.GetAxisRaw("Horizontal");
            rb.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            if (rb.velocity.x > maxSpeed) { rb.velocity = new Vector2(maxSpeed, rb.velocity.y); }
            else if (rb.velocity.x < -maxSpeed) { rb.velocity = new Vector2(-maxSpeed, rb.velocity.y); }

            if (rb.velocity.normalized.x != 0)
                animator.SetBool("isRunning", true);
            else
                animator.SetBool("isRunning", false);
        }
    }
}