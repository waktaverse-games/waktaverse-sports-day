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
            transform.DOMoveY(4, 1).SetRelative().SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        public void Kill()
        {
            DOTween.Kill(gameObject.transform);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player")
            {
                if (code == CoinCode.Bronze) Manager.AddGold(1);
                else if (code == CoinCode.Silver) Manager.AddGold(10);
                else Manager.AddGold(25);

                Manager.ObjectController.Stars.Remove(gameObject);
                Kill();
            }
        }
    }
}

