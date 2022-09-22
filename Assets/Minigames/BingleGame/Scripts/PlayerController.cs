using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] float slowDownSpeed;
        [SerializeField] float controlCoolDown;
        [SerializeField] bool ableToMove;

        private float curTime = 0;
        private bool movingLeft;

        Rigidbody2D rb;
        SpriteRenderer sprite;

        private Vector2 dir = Vector2.right;

        [SerializeField] AudioSource turnSFX;
        [SerializeField] AudioSource itemSFX;
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            ableToMove = true;
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
            CheckCoolTime();
            if(Input.GetKeyDown(KeyCode.Space) && ableToMove)
            {
                turnSFX.Play();

                ableToMove = false;
                curTime = 0;
                movingLeft = !movingLeft;
                rb.velocity = dir * Mathf.Abs(rb.velocity.x * 0.8f);
                if(movingLeft)
                {
                    dir = Vector2.left;
                }
                else
                {
                    dir = Vector2.right;
                }
            }
        }
        private void CheckCoolTime()
        {
            curTime += Time.deltaTime;
            if(curTime >= controlCoolDown)
            {
                ableToMove = true;
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
            else if(collision.gameObject.tag == "Coin")
            {
                itemSFX.Play();
                Destroy(collision.gameObject);  // 코인 파괴
                // 이후 코인 스코어 관리 코드...
            }    
        }
    }
}
