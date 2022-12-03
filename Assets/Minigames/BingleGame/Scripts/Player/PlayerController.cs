using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace GameHeaven.BingleGame
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] GameObject skiEquip;

        [SerializeField] float xSpeed;
        [SerializeField] float ySpeed;
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
            //rb.velocity = dir * 2;
        }

        void Update()
        {
            if (!GameManager.instance.isGameStart)
                return;
            if (GameManager.instance.isGameOver)
            {
                xSpeed = 0;
                ySpeed = 0;
                rb.velocity = Vector2.zero;
                return;
            }
            xSpeed = GameSpeedController.instance.xSpeed;
            ySpeed = GameSpeedController.instance.ySpeed * -1f;
            ChangeDirection();
            FlipSprite();
        }
        void FixedUpdate()
        {
            rb.velocity += new Vector2(dir.normalized.x * xSpeed, 0);
            rb.velocity = new Vector2(rb.velocity.x, ySpeed);
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
                rb.velocity = new Vector2(rb.velocity.x * slowDownSpeed, rb.velocity.y);
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
            sprite.flipX = rb.velocity.x > 0 ? true : false;
            skiEquip.transform.localScale = rb.velocity.x > 0 ? new Vector3(-1, 1, 1) : Vector3.one;
        }
    }
}