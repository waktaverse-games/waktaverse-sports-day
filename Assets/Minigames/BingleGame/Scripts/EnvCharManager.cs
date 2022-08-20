using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{ 
    public class EnvCharManager : MonoBehaviour
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
                transform.Translate(Vector3.right * movingSpeed * Time.deltaTime + Vector3.up * GameSpeedController.instance.speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * movingSpeed * Time.deltaTime + Vector3.up * GameSpeedController.instance.speed * Time.deltaTime);
            }
        }
    }
}

