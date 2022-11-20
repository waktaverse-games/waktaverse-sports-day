using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] GameObject VFX;
        SpriteRenderer spriteRenderer;
        Animator animator;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = VFX.GetComponent<Animator>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                SoundManager.instance.PlayItemSound();
                spriteRenderer.enabled = false;
                StartCoroutine(PlayAnimation());
            }
        }
        IEnumerator PlayAnimation()
        {
            VFX.SetActive(true);
            animator.Play("ItemGain");
            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                yield return null;
            }
            VFX.SetActive(false);
        }
        public void ResetItem()
        {
            spriteRenderer.enabled = true;
            VFX.SetActive(false);
        }
    }
}
