using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] GameObject skiEquip;

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
                dir = movingLeft ? Vector2.left : Vector2.right;
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
            sprite.flipX = rb.velocity.x >= 0 ? true : false;
            skiEquip.transform.localScale = rb.velocity.x >= 0 ? new Vector3(-1,1,1) : Vector3.one;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "GameOverArea" || collision.gameObject.tag == "Border")
            {
                GameManager.instance.GameOver();
            }
            else if(collision.gameObject.tag == "Coin")
            {
                itemSFX.Play();
                Destroy(collision.gameObject);
            }
        }
    }
}
