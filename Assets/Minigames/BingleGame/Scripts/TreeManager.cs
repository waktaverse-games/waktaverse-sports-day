using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameHeaven.BingleGame.Enums;

namespace GameHeaven.BingleGame
{
    public class TreeManager : MonoBehaviour
    {
        public int score;
        public float throwingSpeed;

        public GameObject[] characters;

        [SerializeField] Sprite[] treeSprites;

        Rigidbody2D rigid;
        Vector3 initialPos;
        TreeType treeType;
        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            initialPos = transform.localPosition ;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameManager.instance.IncreaseScore(score);
                SoundManager.instance.PlayCrashSound();
                // 체크포인트의 다른 콜라이더 비활성화
                transform.parent.GetComponent<CheckPointManager>().DisableOtherCollider();

                ThrowTree(collision, out Vector2 charDir);
                GenerateCharacter();
            }
        }

        private void ThrowTree(Collider2D collision, out Vector2 charDir)
        {
            // Throw away this object
            Vector3 playerPos = collision.transform.position;
            Vector3 direction = transform.position - playerPos;
            charDir = direction.x >= 0 ? new Vector2(1, -1) : new Vector2(-1, -1);

            rigid.velocity = direction.normalized * throwingSpeed;
        }

        private void GenerateCharacter()
        {
            int randIdx = Random.Range(0, characters.Length);
            GameObject character = Instantiate(characters[randIdx], transform.position, transform.rotation);
            if(transform.gameObject.name == "Tree Right")
            {
                character.GetComponent<SpriteRenderer>().flipX = true;
            }

        }

        public void SetTreeType(int type)
        {
            transform.GetComponent<SpriteRenderer>().sprite = treeSprites[type];
            treeType = (TreeType)type;
        }

        public void ResetTree()
        {
            rigid.velocity = Vector2.zero;  // 속도 0으로
            transform.localPosition = initialPos;    // 최초 포지션으로
        }
    }
}
