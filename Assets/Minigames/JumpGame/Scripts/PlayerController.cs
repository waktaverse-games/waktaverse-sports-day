using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class PlayerController : MonoBehaviour
    {
        public float maxSpeed;
        public float jumpPower;
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
            if(Input.GetButtonDown("Jump") && !animator.GetBool("isJumping"))
            {
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                animator.SetBool("isJumping", true);
            }
            if(rb.velocity.y == 0)
            {
                animator.SetBool("isJumping", false);
            }
            UpdateFace();
        }
        private void FixedUpdate()
        {
            PlayerMovement();
            PlayerLanding();
        }

        void UpdateFace()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                sprite.flipX = false;
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                sprite.flipX = true; 
            }
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

        void PlayerLanding()
        {
            if (rb.velocity.y < 0)
            {
                Debug.DrawRay(rb.position, Vector3.down, new Color(1, 0, 0));
                RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector3.down, 1, LayerMask.GetMask("Ground"));
                if (rayHit.collider != null)
                {
                    if (rayHit.distance < 1f)
                    {
                        animator.SetBool("isJumping", false);
                    }
                }
            }
        }
    }
}