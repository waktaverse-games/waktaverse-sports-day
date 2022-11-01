using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{ 
    public class NPCManager : MonoBehaviour
    {
        SpriteRenderer renderer;
        Vector2 dir;

        public float movingSpeed;
        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if(renderer.flipX)
            {
                transform.Translate(Vector3.right * movingSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * movingSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Border")
            {
                Destroy(gameObject);
            }
        }
    }
}
