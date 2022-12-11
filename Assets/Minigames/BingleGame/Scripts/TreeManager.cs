using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class TreeManager : MonoBehaviour
    {
        public float throwingSpeed;

        public GameObject[] characters;
        [SerializeField] GameObject hitVFX;
        [SerializeField] Sprite[] flagSprites;

        Rigidbody2D rigid;
        Vector3 initialPos;
        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            initialPos = transform.localPosition ;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                SoundManager.instance.PlayCrashSound();
                // 체크포인트의 다른 콜라이더 비활성화
                transform.parent.GetComponent<CheckPointManager>().DisableOtherCollider();

                GameObject vfx = Instantiate(hitVFX, transform.position, transform.rotation);
                Destroy(vfx, 3f);
                ThrowTree(collision, out Vector2 charDir);
                GenerateCharacter();
            }
        }

        private void ThrowTree(Collider2D collision, out Vector2 charDir)
        {
            Vector3 playerPos = collision.transform.position;
            Vector3 direction = transform.position - playerPos;
            charDir = direction.x >= 0 ? new Vector2(1, -1) : new Vector2(-1, -1);

            rigid.velocity = direction.normalized * throwingSpeed;
        }

        private void GenerateCharacter()
        {
            int randIdx = Random.Range(0, characters.Length);
            GameObject character = Instantiate(characters[randIdx], transform.position, transform.rotation);
            bool isPerson = transform.gameObject.name == "Tree Left" ? true : false;
            character.GetComponent<NPCManager>().Initialize(isPerson);

            if(transform.gameObject.name == "Tree Right")
            {
                character.GetComponent<SpriteRenderer>().flipX = true;
            }

        }

        public void SetTreeType(int type)
        {
            transform.GetComponent<SpriteRenderer>().sprite = flagSprites[type];
        }

        public void ResetTree()
        {
            rigid.velocity = Vector2.zero;  // 속도 0으로
            transform.localPosition = initialPos;    // 최초 포지션으로
        }
    }
}
