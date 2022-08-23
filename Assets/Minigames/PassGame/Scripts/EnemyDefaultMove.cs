using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefaultMove : MonoBehaviour
{
    public float speed = 5.0f;
    public GameObject player;
    public GameManager gameManager;
    public float deletePosX;

    private Rigidbody2D _rigidbody;
    private bool _passed = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = new Vector2(-speed, 0);
    }

    private void OnDisable()
    {
        _passed = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckScore();
    }

    void CheckScore()
    {
        if (_passed) return;
        if (transform.position.x < player.transform.position.x)
        {
            _passed = true;
            gameManager.AddScore(10);
        }

        if (transform.position.x < deletePosX)
        {
            gameObject.SetActive(false);
        }
    }
}
