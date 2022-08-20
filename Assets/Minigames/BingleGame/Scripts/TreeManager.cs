using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class TreeManager : MonoBehaviour
    {
        public int score;
        public float throwingSpeed;

        public GameObject[] characters;

        Rigidbody2D rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                // Send score to the Gamemanager
                GameManager.instance.IncreaseScore(score);
                // Disable other objects' collider
                transform.parent.GetComponent<CheckPointManager>().DisableOtherCollider();

                // Throw away this object
                Vector3 playerPos = collision.transform.position;
                Vector3 direction = transform.position - playerPos;
                Vector2 charDir;
                if (direction.x >= 0)
                    charDir = Vector2.right;
                else
                    charDir = Vector2.left;

                rigid.velocity = direction.normalized * throwingSpeed;

                GenerateCharacter(charDir);

                Invoke("DestroyThisObject",3f);
            }
        }

        private void GenerateCharacter(Vector2 dir)
        {
            int randIdx = Random.Range(0, characters.Length);
            GameObject character = Instantiate(characters[randIdx], transform.position, transform.rotation);
            if(transform.gameObject.name == "Tree Right")
            {
                character.GetComponent<SpriteRenderer>().flipX = true;
            }

        }
        private void DestroyThisObject()
        {
            Destroy(gameObject);
        }
    }
}
