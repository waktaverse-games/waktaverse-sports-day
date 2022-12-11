using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{ 
    public class NPCManager : MonoBehaviour
    {
        Animator anim;
        [SerializeField] RuntimeAnimatorController[] animatorControllers;

        SpriteRenderer spriteRenderer;
        Vector2 dir;

        public float movingSpeed;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
        }
        public void Initialize(bool isPerson)
        {
            if(isPerson)
                anim.runtimeAnimatorController = animatorControllers[Random.Range(0, 7)];
            else
                anim.runtimeAnimatorController = animatorControllers[Random.Range(7, animatorControllers.Length)];
        }

        void Update()
        {
            if(spriteRenderer.flipX)
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

