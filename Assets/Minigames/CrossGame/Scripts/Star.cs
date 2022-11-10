using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameHeaven.CrossGame
{
    public class Star : MonoBehaviour
    {
        public CrossGameManager Manager;
        public CoinCode code; 
        Animator MyAnimator;
        private void Awake()
        {
            MyAnimator = GetComponent<Animator>();
        }
        public void SetAnim()
        {
            MyAnimator.SetInteger("Sort", (int)code);
        }
        public void Move()
        {
            //transform.DOMoveY(4, 1).SetRelative().SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        public void Kill()
        {
            DOTween.Kill(gameObject.transform);
            Manager.ObjectController.Stars.Remove(gameObject);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player")
            {
                Manager.AddScore(10);
                Manager.SoundManager.Play("Coin");
                Kill();
            }
            else if(collision.tag == "Outline")
            {
                Kill();
            }
        }
    }
}

