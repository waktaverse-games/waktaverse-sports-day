using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameHeaven.JumpGame
{
    public class PlayerController : MonoBehaviour
    {
        public UnityEvent OnCollideWithRope;
        public GameObject VFX;

        public float maxSpeed;
        public float jumpPower;
        Rigidbody2D rb;
        SpriteRenderer sprite;
        Animator animator;

        bool isImmotal = false;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            PlayerBrake();
            PlayerJump();
            UpdateFace();
        }
        private void FixedUpdate()
        {
            PlayerMovement();
            PlayerLanding();
        }
        void PlayerBrake()
        {
            if (Input.GetButtonUp("Horizontal"))
            {
                rb.velocity = new Vector2(rb.velocity.normalized.x * 0.5f, rb.velocity.y);
            }
        }
        void PlayerJump()
        {
            if (Input.GetButtonDown("Jump") && !animator.GetBool("isJumping") && rb.velocity.y <= 0.2f)
            {
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                animator.SetBool("isJumping", true);
                SoundManager.Instance.PlayJumpSound();
            }
            if (rb.velocity.y == 0)
            {
                animator.SetBool("isJumping", false);
            }
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

            if (rb.velocity.normalized.x != 0) { animator.SetBool("isRunning", true); }
            else { animator.SetBool("isRunning", false); }
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

        public void EnableExclamationMark()
        {
            VFX.SetActive(true);
            VFX.GetComponent<Animator>().Play("ExclamationMark");
            Invoke("DisableExclamationMark", 1f);
        }
        void DisableExclamationMark()
        {
            VFX.SetActive(false);
        }

        IEnumerator OnDamaged()
        {
            isImmotal = true;
            int countTime = 0;
            while (countTime < 40)
            {
                if(countTime % 2 == 0) { sprite.color = new Color(1, 1, 1, 0.4f); }
                else { sprite.color = new Color(1, 1, 1, 1); }
                yield return new WaitForSeconds(0.05f);
                countTime++;
            }
            isImmotal = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.name.Equals("Rope Collider"))
            {
                if(GameManager.Instance.isInvincible) // 무적이면
                {
                    GameManager.Instance.isInvincible = false;
                    OnCollideWithRope.Invoke();
                    StartCoroutine(OnDamaged());
                }
                else if(!isImmotal)
                {
                    animator.SetBool("isGameOver", true);
                    GameManager.Instance.GameOver();
                }
            }
        }


    }
}