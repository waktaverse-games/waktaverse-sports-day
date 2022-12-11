using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameHeaven.CrossGame
{
    public class Star : MonoBehaviour
    {
        public CrossGameManager manager;
        public CoinCode code; 
        public List<Sprite> sprites;
        //Animator MyAnimator;
        private void Awake()
        {
            //MyAnimator = GetComponent<Animator>();
        }
        public void SetAnim()
        {
            //MyAnimator.SetInteger("Sort", (int)code);
            gameObject.GetComponent<SpriteRenderer>().sprite = sprites[(int)code];
        }
        public void Move()
        {
            //transform.DOMoveY(4, 1).SetRelative().SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        public void Kill()
        {
            DOTween.Kill(gameObject.transform);
            manager.objectController.stars.Remove(gameObject);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player")
            {
                manager.AddScore(10);
                manager.soundManager.SfxPlay("Coin");
                Kill();
            }
            else if(collision.tag == "Outline")
            {
                Kill();
            }
        }
    }
}

