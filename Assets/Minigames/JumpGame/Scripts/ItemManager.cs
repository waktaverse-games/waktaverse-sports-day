using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] GameObject VFX;
        [SerializeField] int score;
        [SerializeField]
        Sprite[] itemSprites;
        ItemSpawner spawner;
        SpriteRenderer spriteRenderer;
        //Animator animator;
        private void Awake()
        {
            //animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void InitializeItem(ItemSpawner _spawner, int itemNum)
        {
            spawner = _spawner;
            //animator.SetInteger("itemNum", itemNum);
            spriteRenderer.sprite = itemSprites[itemNum];
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                SoundManager.Instance.PlayItemSound();
                GameManager.Instance.IncreaseScore(score);
                GameObject vfx = Instantiate(VFX, transform.position, transform.rotation);
                Destroy(vfx, 2f);
                spawner.DeactiavteItem(gameObject);
            }
            if(collision.gameObject.tag == "Border")
            {
                spawner.DeactiavteItem(gameObject);
            }
        }
    }
}
