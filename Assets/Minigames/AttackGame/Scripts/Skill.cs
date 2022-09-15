using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace GameHeaven.AttackGame
{
    public class Skill : MonoBehaviour
    {
        public int tweenId;
        public Player player;
        
        public bool isArrow = true;
        private Tween _tween;

        private void OnEnable()
        {
            StartCoroutine(FallDown());
        }

        private void OnDisable()
        {
            _tween.Kill();
            transform.position = new Vector3(transform.position.x, 4);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameObject gameObj = col.gameObject;
            if (gameObj.CompareTag("Enemy"))
            {
                if (isArrow)
                {
                    player.WeaponFree(2);
                }
                else
                {
                    player.WeaponFree(3);
                }
            }
        }

        IEnumerator FallDown()
        {
            yield return new WaitForSeconds(3f);
            _tween = transform.DOMoveY(1f, 1);
        }
    }
}