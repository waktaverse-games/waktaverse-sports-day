using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class MonsterBase : MonoBehaviour
    {
        public float MoveSpeed = 0f;
        public Vector2 StartPosition;
        
        private void OnEnable()
        {
            transform.position = StartPosition;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector2.left * Time.deltaTime * MoveSpeed);

            if (transform.position.x < -10f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
