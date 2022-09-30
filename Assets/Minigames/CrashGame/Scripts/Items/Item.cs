using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public abstract class Item : MonoBehaviour, IDestroyEffect
    {
        // Parent Class of all items
        private GameObject destroyEffect;

        public abstract void ActivateItem();        // æ∆¿Ã≈€ »πµÊ Ω√ ¡Ôπﬂ

        protected virtual void Awake()
        {
            destroyEffect = transform.GetChild(0).gameObject;
            Debug.Log($"{this}.{destroyEffect}");
        }

        public void DestroyItem()
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            destroyEffect.SetActive(true);
        }

        public void DestroyAfterEffect()
        {
            Destroy(gameObject);
        }
    }

}
