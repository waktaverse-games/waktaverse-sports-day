using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace GameHeaven.AttackGame
{
    public class Enemy : MonoBehaviour
    {
        public GameObject player;
        public GameManager gameManager;
        public ObjectManager objectManager;
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

        public void SetState(bool isBoss, int hp)
        {
            totalHp = hp;
            hpBar.fillAmount = 1f;
            currentHp = totalHp;
            if (isBoss)
            {
                Vector3 temp = transform.localScale;
            }
        }

        public void HitByProjectile(int damage)
        {
            currentHp -= damage;
            if (currentHp < 0)
            {
                currentHp = 0;
                Invoke("DisableObject", 0.2f);
            }
            hpBar.fillAmount = (float)currentHp / totalHp;
        }

        public void DisableObject()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        public void ActivateMovement()
        {
            switch (_name)
            {
                case "monkey(Clone)":
                    return;
                case "fox(Clone)":
                    StartCoroutine(FoxMove());
                    break;
                case "gorani(Clone)":
                    StartCoroutine(GoraniToLeft());
                    break;
                case "pigeon(Clone)":
                    StartCoroutine(PigeonMove());
                    break;
                case "cat(Clone)":
                    StartCoroutine(CatJump());
                    break;
                case "bat(Clone)":
                    StartCoroutine(BatMove());
                    break;
                case "dog(Clone)":
                    StartCoroutine(DogThrow());
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

        IEnumerator BatMove()
        {
            yield return new WaitForSeconds(1f);
            Vector3 pos = player.transform.position;
            Vector3 newPos = new Vector3(Random.Range(pos.x - 3.5f, pos.x + 3.5f), Random.Range(pos.y - 0.2f, pos.y + 1.2f),
                pos.z);
            if (newPos.x > transform.position.x)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }

            transform.DOMove(newPos, 2);
            yield return new WaitForSeconds(0.9f);
            StartCoroutine(BatMove());
            
        }

        IEnumerator DogThrow()
        {
            yield return new WaitForSeconds(2);
            GameObject poop = objectManager.MakeObject("poop", transform.position);
            poop.GetComponent<Projectile>().PoopThrow();
            StartCoroutine(DogThrow());
        }
    }
}

