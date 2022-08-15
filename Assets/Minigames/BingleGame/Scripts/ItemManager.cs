using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] float speed;


        void Awake()
        {
            speed = GameSpeedController.instance.speed;
        }

        void Update()
        {
            speed = GameSpeedController.instance.speed;
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

            if (collision.gameObject.tag == "Player")
            {
                // 특별한 이펙트? 같은거 넣어야하나...
                Destroy(gameObject);
            }
        }
    }
}
