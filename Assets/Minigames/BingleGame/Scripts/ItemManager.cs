using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] GameObject VFX;
        SpriteRenderer spriteRenderer;
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnCollision()
        {
            SoundManager.instance.PlayItemSound();

            spriteRenderer.enabled = false;
            GameObject vfx = Instantiate(VFX, transform.position, transform.rotation);
            Destroy(vfx, 3f);
        }

    }
}
