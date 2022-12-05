using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class Background : MonoBehaviour
    {
        private BoxCollider2D _boxCollider;
        private Rigidbody2D _rigidbody;
        private float width;

        public float speed = 3f;
        public bool shouldMove;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            width = _boxCollider.size.x;
            _rigidbody.velocity = new Vector2(-speed, 0);
            shouldMove = true;
        }

        void Update()
        {
            if (transform.position.x < -width && shouldMove)
            {
                Reposition();
            }
        }

        private void Reposition()
        {
            Vector2 vector = new Vector2(width * 2.998f, 0);
            transform.position = (Vector2)transform.position + vector;
        }
    }
}