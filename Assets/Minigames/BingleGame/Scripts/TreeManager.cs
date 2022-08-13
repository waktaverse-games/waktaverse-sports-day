using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class TreeManager : MonoBehaviour
    {
        public int score;
        public float throwingSpeed;
        Rigidbody2D rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Collision!");
                // Send score to the Gamemanager
                GameManager.instance.IncreaseScore(score);
                // Disable other objects' collider
                transform.parent.GetComponent<CheckPointManager>().DisableOtherCollider();

                // Throw away this object
                Vector3 playerPos = collision.transform.position;
                Vector3 direction = transform.position - playerPos;

                rigid.velocity = direction.normalized * throwingSpeed;


                Invoke("DestroyThisObject",3f);
            }
        }

        private void DestroyThisObject()
        {
            Destroy(gameObject);
        }
    }
}
