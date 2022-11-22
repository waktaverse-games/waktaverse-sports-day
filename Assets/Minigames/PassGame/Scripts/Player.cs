using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class Player : MonoBehaviour
    {
        public float jumpPower = 3.0f;
        public bool reachedJump = false;
        public GameManager gameManager;
        public SFXManager SfxManager;
        public bool jumpItem = false;
        public GameObject boomEffect;
        
        private Rigidbody2D _rigid;
        private bool _isGrounded = false;
        private Animator _anim;

        // Start is called before the first frame update
        void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            Jump();
            if (transform.position.y > -3.39f) _anim.SetBool("isJump", true);
        }

        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            {
                _rigid.velocity = Vector2.up * jumpPower;
                SfxManager.PlaySfx(0);
                _anim.SetBool("isJump", true);
                _isGrounded = false;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && jumpItem)
            {
                _rigid.velocity = Vector2.up * jumpPower;
                SfxManager.PlaySfx(0);
                _anim.SetBool("isJump", true);
                _isGrounded = false;
                jumpItem = false;
                gameManager.ItemDeactivate();
            }
        }

        public void ResetGame()
        {
            _anim.SetBool("isJump", false);
            _isGrounded = true;
            _rigid.velocity = Vector2.zero;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Bottom"))
            {
                _isGrounded = true;
                reachedJump = false;
                _anim.SetBool("isJump", false);
            }
            
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                _rigid.velocity = Vector2.up * jumpPower;
                SfxManager.PlaySfx(1);
                gameManager.AddScore(15);
                boomEffect.SetActive(true);
                boomEffect.GetComponent<Animator>().Play("effect");
                Invoke("DisableEffect", 0.3f);
            }
        }

        private void DisableEffect()
        {
            boomEffect.SetActive(false);
        }
    }
}