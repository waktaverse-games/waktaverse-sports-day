using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameHeaven.PassGame
{
    public class EnemyPattern : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private float _count = 1;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            _count = 1;
            switch (gameObject.name)
            {
                case "segyun":
                    StartCoroutine(Segyun(1f));
                    break;
                case "gorani":
                    StartCoroutine(Gorani(1.5f));
                    break;
                case "fox":
                    transform.localPosition = new Vector3(-14.8f, 0, 0);
                    _animator.SetBool("isPause", true);
                    StartCoroutine(Fox(1.5f));
                    break;
                case "ddulgi":
                    _animator.SetBool("isFly", false);
                    StartCoroutine(Ddulgi());
                    break;
                case "dog":
                    _animator.SetBool("isFly", false);
                    StartCoroutine(Dog());
                    break;
                case "bat":
                    StartCoroutine(Bat());
                    break;
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            transform.position = transform.parent.position;
            _spriteRenderer.flipX = false;
        }

        IEnumerator Panchi()
        {
            yield break;
        }

        IEnumerator Bat()
        {
            yield return new WaitForSeconds(5f);
            transform.DOMoveY(-1f, 1);
        }

        IEnumerator Segyun(float time)
        {
            yield return new WaitForSeconds(time);
            transform.DOJump(transform.parent.position + new Vector3(-3, 0, 0), 1, 1, 1);
            StartCoroutine(Segyun(1f));
        }

        IEnumerator Gorani(float time)
        {
            yield return new WaitForSeconds(time);
            if (transform.parent.position.x > -8f)
            {
                _spriteRenderer.flipX = true;
                transform.DOLocalMoveX(3f, 0.5f);
                StartCoroutine(GoraniLeft(0.8f));
            }
        }

        IEnumerator GoraniLeft(float time)
        {
            yield return new WaitForSeconds(time);
            _spriteRenderer.flipX = false;
            transform.DOLocalMoveX(-1.5f, 1);
            StartCoroutine(Gorani(1.3f));
        }

        IEnumerator Fox(float time)
        {
            yield return new WaitForSeconds(time);
            _animator.SetBool("isPause", false);
            Invoke("FoxStop", 1f);
            transform.DOLocalMoveX(-3.7f * _count, 1);
            // Debug.Log(_count);
            _count++;
            StartCoroutine(Fox(2.5f));
        }

        void FoxStop()
        {
            _animator.SetBool("isPause", true);
        }

        IEnumerator Ddulgi()
        {
            yield return new WaitForSeconds(3f);
            _animator.SetBool("isFly", true);
            transform.DOLocalMoveX(-10f, 4);
        }

        IEnumerator Dog()
        {
            yield return new WaitForSeconds(2f);
            _animator.SetBool("isFly", true);
            StartCoroutine(DogMove());
            StartCoroutine(DogWake());
        }

        IEnumerator DogMove()
        {
            yield return new WaitForSeconds(0.12f);
            transform.Translate(-0.6f, 0f, 0f);
        }

        IEnumerator DogWake()
        {
            yield return new WaitForSeconds(6.5f);
            transform.Translate(0.6f, 0f, 0);
        }
    }
}