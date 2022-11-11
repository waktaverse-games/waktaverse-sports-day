using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SharedLibs;
using SharedLibs.Character;

namespace GameHeaven.CrossGame
{
    public enum PlatformType{
        Viichan,
        Gosegu1,
        Gosegu2,
        Jururu,
        Lilpa,
        Jingburger,
        Ine
    }
    public class Platform : MonoBehaviour
    {
        public PlatformType type;
        public List<Sprite> normalSprite = new List<Sprite>();
        public List<Sprite> pushedSprite = new List<Sprite>();
        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            Set();
        }

        public void Set()
        {
            int num = Random.Range(0, 12);

            if (num < 2) type = PlatformType.Viichan;
            else if (num < 3) type = PlatformType.Gosegu1;
            else if (num < 4) type = PlatformType.Gosegu2;
            else if (num < 6) type = PlatformType.Jururu;
            else if (num < 8) type = PlatformType.Lilpa;
            else if (num < 10) type = PlatformType.Jingburger;
            else if (num < 12) type = PlatformType.Ine;

            spriteRenderer.sprite = normalSprite[(int)type];
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                EnterPush();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                ExitPush();
            }
        }

        public void EnterPush(){
            //transform.DOScale(new Vector3(0.3f, 0.2f), 0.5f).SetEase(Ease.OutBounce);
            spriteRenderer.sprite = pushedSprite[(int)type];
        }

        public void ExitPush(){
            Sequence sequence = DOTween.Sequence();
            //sequence.Append(transform.DOScale(new Vector3(0.2f, 0.375f), 0.2f).SetEase(Ease.OutBounce)).Append(transform.DOScale(new Vector3(0.25f, 0.25f), 0.5f).SetEase(Ease.OutBounce));
            spriteRenderer.sprite = normalSprite[(int)type];
        }
    }
}
