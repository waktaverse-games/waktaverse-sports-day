using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class CheckPointManager : MonoBehaviour
    {
        public float speed;
        public GameObject[] children;
        void Update()
        {
            Vector3 curPos = transform.position;
            Vector3 nextPos = Vector3.up * speed * Time.deltaTime;
            transform.position = curPos + nextPos;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Border")
            {
                Destroy(gameObject);
            }
        }

        public void DisableOtherCollider()
        {
            foreach(var obj in children)
            {
                if (obj.GetComponent<BoxCollider2D>())
                {
                    obj.GetComponent<BoxCollider2D>().enabled = false;
                }
                else
                {
                    obj.GetComponent<CapsuleCollider2D>().enabled = false;
                }
            }
        }
    }
}