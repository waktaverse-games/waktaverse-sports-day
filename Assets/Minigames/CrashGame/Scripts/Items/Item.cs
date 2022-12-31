using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public abstract class Item : MonoBehaviour, IDestroyEffect
    {
        // Parent Class of all items

        private GameObject destroyEffect;

        public abstract string GetName();
        public abstract void ActivateItem();        // æ∆¿Ã≈€ »πµÊ Ω√ ¡Ôπﬂ

        protected virtual void Awake()
        {
            destroyEffect = transform.GetChild(0).gameObject;
            //Debug.Log($"{this}.{destroyEffect}");
        }

        protected void OnEnable()
        {
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            GetComponent<BoxCollider2D>().enabled = true;
            destroyEffect.SetActive(false);
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
            MiniGameManager.ObjectPool.ReturnObject(GetName(), gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //if (collision.collider.tag == "Bottom")
            //{
            //    StartCoroutine(TimeoutDestroy(2));
            //}
        }

        private IEnumerator TimeoutDestroy(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            MiniGameManager.ObjectPool.ReturnObject(GetName(), gameObject);
        }
    }

}
