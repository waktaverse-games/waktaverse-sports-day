using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class NewPlayerController : MonoBehaviour
    {
        [SerializeField] GameObject skiEquip;

        [SerializeField] float speed;
        [SerializeField] float slowDownSpeed;
        [SerializeField] float controlCoolDown;
        [SerializeField] bool ableToMove;

        private float curTime = 0;
        private bool movingLeft = true;

        Rigidbody2D rb;
        SpriteRenderer sprite;

        private Vector2 dir = new Vector2(-1,-1);

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            ableToMove = true;
            rb.velocity = dir * 5;
        }

        void Update()
        {
            speed = GameSpeedController.instance.speed;
            ChangeDirection();
            FlipSprite();
        }
        void FixedUpdate()
        {
            rb.velocity += new Vector2(dir.normalized.x * speed, 0);
        }

        private void ChangeDirection()
        {
            CheckCoolTime();
            if (Input.GetKeyDown(KeyCode.Space) && ableToMove)
            {
                SoundManager.instance.PlayTurnSound();
                ableToMove = false;
                curTime = 0;
                movingLeft = !movingLeft;
                rb.velocity = new Vector2(rb.velocity.x * 0.8f, rb.velocity.y);
                dir = movingLeft ? new Vector2(-1,-1) : new Vector2(1,-1);
            }
        }
        private void CheckCoolTime()
        {
            curTime += Time.deltaTime;
            if (curTime >= controlCoolDown)
            {
                ableToMove = true;
            }
        }

        private void FlipSprite()
        {
            sprite.flipX = rb.velocity.x >= 0 ? true : false;
            skiEquip.transform.localScale = rb.velocity.x >= 0 ? new Vector3(-1, 1, 1) : Vector3.one;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "GameOverArea" || collision.gameObject.tag == "Border")
            {
                GameManager.instance.GameOver();
            }
        }
    }
}