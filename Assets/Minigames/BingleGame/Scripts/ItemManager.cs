using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] GameObject VFX;
        [SerializeField] int score;
        SpriteRenderer spriteRenderer;
        Animator animator;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            //animator = VFX.GetComponent<Animator>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                SoundManager.instance.PlayItemSound();
                GameManager.instance.IncreaseScore(score);
                spriteRenderer.enabled = false;
                GameObject vfx = Instantiate(VFX, transform.position, transform.rotation);
                Destroy(vfx, 3f);
            }
        }

        public void ResetItem()
        {
            spriteRenderer.enabled = true;
        }
    }
}
