using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameHeaven.CrossGame
{
    public class FlyItem : MonoBehaviour
    {
        public CrossGameManager Manager;
        public Sequence seq;
        public void Move()
        {
            transform.DOMoveY(2, 0.6f).SetRelative().SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        public void Stop()
        {
            DOTween.Kill(gameObject.transform);
        }

        public void Kill()
        {
            DOTween.Kill(gameObject.transform);
            Manager.ObjectController.FlyItems.Remove(gameObject);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                Manager.ObjectController.Fly();
                Kill();
            }
            else if(collision.tag == "Outline")
            {
                Kill();
            }
        }
    }
}