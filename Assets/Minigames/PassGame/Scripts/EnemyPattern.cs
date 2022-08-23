using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyPattern : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        switch (gameObject.name)
        {
            case "segyun":
                StartCoroutine(Segyun(1f));
                break;
            case "gorani":
                StartCoroutine(Gorani(1.5f));
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

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

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1);
        Debug.Log(transform.parent.position);
        Debug.Log(transform.position);
    }
}
