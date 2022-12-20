using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class EnemyDefaultMove : MonoBehaviour
    {
        public float speed = 3.0f;
        public GameObject player;
        public GameManager gameManager;
        public float deletePosX;

        private Rigidbody2D _rigidbody;

        public bool _passed = false;

        // Start is called before the first frame update
        void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.velocity = new Vector2(-speed, 0);
            _passed = false;
            StartCoroutine(CheckScoreEnum());
            // transform.Translate(0,0.3f,0);
        }

        private void OnDisable()
        {
            _passed = false;
            StopAllCoroutines();
        }

        // Update is called once per frame
        void Update()
        {
            CheckDisablePosition();
            // CheckScore();
        }

        void CheckDisablePosition()
        {
            if (transform.position.x < deletePosX)
            {
                gameObject.SetActive(false);
            }
        }

        IEnumerator CheckScoreEnum()
        {
            yield return new WaitForSeconds(0.06f);
            if (transform.position.x < player.transform.position.x && !_passed)
            {
                Debug.Log(_passed);
                _passed = true;
                gameManager.AddScore(10);
            }
            else
            {
                StartCoroutine(CheckScoreEnum());
            }
        }

        void CheckScore()
        {
            if (_passed) return;
            if (transform.position.x < player.transform.position.x && !_passed)
            {
                Debug.Log(_passed);
                _passed = true;
                gameManager.AddScore(10);
            }
        }
    }
}