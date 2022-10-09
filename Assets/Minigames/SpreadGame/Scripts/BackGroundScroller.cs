using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.StickyGame
{
    public class BackGroundScroller : MonoBehaviour
    {
        public float speed;

        private void Update()
        {
            transform.position += Vector3.left * (Time.deltaTime * speed);

            if (transform.position.x < -15)
            {
                transform.position = new Vector3(15, transform.position.y, transform.position.z);
            }
        }
    }
}
