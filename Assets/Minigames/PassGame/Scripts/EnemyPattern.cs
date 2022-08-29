using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
                _animator.SetBool("isPause", false);
                StartCoroutine(Fox(1.5f));
                break;
            case "ddulgi":
                _animator.SetBool("isFly", false);
                StartCoroutine(Ddulgi());
                break;
            case "dog":
                StartCoroutine(Dog());
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

    IEnumerator Segyun(float time)
    {
        yield return new WaitForSeconds(time);
        transform.DOJump(transform.parent.position + new Vector3(-3, 0, 0),1, 1, 1);
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
        Invoke("FoxGo", 2.2f);
        Invoke("FoxStop", 0.7f);
        transform.DOLocalMoveX(-3.7f * _count, 1);
        _count++;
        StartCoroutine(Fox(2.5f));
    }

    void FoxStop()
    {
        _animator.SetBool("isPause", true);
    }

    void FoxGo()
    {
        _animator.SetBool("isPause", false);
    }

    IEnumerator Ddulgi()
    {
        yield return new WaitForSeconds(3f);
        _animator.SetBool("isFly", true);
        Invoke("DdulgiMove", 0.3f);
    }

    void DdulgiMove()
    {
        transform.DOLocalMoveX(-10f, 4);
    }

    IEnumerator Dog()
    {
        yield return new WaitForSeconds(1);
    }
}