using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GameHeaven.AttackGame
{
    public class Enemy : MonoBehaviour
    {
        public GameObject player;
        public GameManager gameManager;
        public Image hpBar;
        public int totalHp;

        private int currentHp;
        private string _name;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _name = gameObject.name;
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void OnEnable()
        {
            hpBar.fillAmount = 1f;
            currentHp = totalHp;
        }

        public void HitByProjectile(int damage)
        {
            currentHp -= damage;
            hpBar.fillAmount = (float)currentHp / totalHp;
        }

        public void DisableObject()
        {
            
        }

        public void ActivateMovement()
        {
            switch (_name)
            {
                case "monkey":
                    return;
                case "fox":
                    StartCoroutine(FoxMove());
                    break;
                case "gorani":
                    StartCoroutine(GoraniToLeft());
                    break;
                case "pigeon":
                    StartCoroutine(PigeonMove());
                    break;
                case "cat":
                    StartCoroutine(CatJump());
                    break;
            }
        }

        IEnumerator FoxMove()
        {
            yield return new WaitForSeconds(0.9f);
            _animator.SetBool("isMove", true);
            StartCoroutine(FoxStop());
            transform.DOLocalMoveX(transform.position.x - 1.5f, 1.2f);
        }

        IEnumerator FoxStop()
        {
            yield return new WaitForSeconds(1f);
            
            _animator.SetBool("isMove", false);
            StartCoroutine(FoxMove());
        }

        IEnumerator GoraniToLeft()
        {
            yield return new WaitForSeconds(1.1f);
            _spriteRenderer.flipX = false;
            transform.DOLocalMoveX(transform.position.x - 1.5f, 1f);
            StartCoroutine(GoraniToRight());
        }

        IEnumerator GoraniToRight()
        {
            yield return new WaitForSeconds(1.1f);
            _spriteRenderer.flipX = true;
            transform.DOLocalMoveX(transform.position.x + 1.5f, 1f);
            StartCoroutine(GoraniToLeft());
        }

        IEnumerator PigeonMove()
        {
            _animator.SetBool("isMove", true);
            yield return new WaitForSeconds(1f);
            Vector3 pos = player.transform.position;
            transform.DOMove(new Vector3(pos.x + 3, pos.y + 2, pos.z), 2);
            StartCoroutine(PigeonStop());
        }

        IEnumerator PigeonStop()
        {
            yield return new WaitForSeconds(2f);
            _animator.SetBool("isMove", false);
        }

        IEnumerator CatJump()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetBool("isMove", true);
            Vector3 pos = player.transform.position;
            StartCoroutine(CatStop());
            transform.DOMoveY(2.5f, 1);
        }

        IEnumerator CatStop()
        {
            yield return new WaitForSeconds(0.9f);
            _animator.SetBool("isMove", false);
            StartCoroutine(CatJump());
        }
    }
}

