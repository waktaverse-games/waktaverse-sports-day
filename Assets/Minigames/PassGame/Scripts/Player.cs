using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigid;
    private bool _isGrounded = false;
    private Animator _anim;
    
    public float jumpPower = 30.0f;
    public GameManager gameManager;
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
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && _isGrounded)
        {
            _rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            _anim.SetBool("isJump", true);
            _isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bottom"))
        {
            _isGrounded = true;
            _anim.SetBool("isJump", false);
        }
        if (col.gameObject.CompareTag("Enemy"))
        {
            _isGrounded = true;
            gameManager.AddScore(15);
        }
    }
}
