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
        public SFXManager sfxManager;
        public int totalHp;
        public bool isBossMonster = false;
        public int damage = 20;
        public int tweenId;
        public bool dropItem = false;
        public GameObject hammerItem;
        
        private int currentHp;
        private string _name;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Tween _tween;
        private bool _isAlive;
        private bool _isMonkeyMove;

        private void Start()
        {
            _name = gameObject.name;
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            isBossMonster = false;
            _isMonkeyMove = false;
            damage = 10;
        }

        public void SetState(bool isBoss, int hp, int enemyDamage)
        {
            totalHp = hp;
            hpBar.fillAmount = 1f;
            currentHp = totalHp;
            damage = enemyDamage;
            _isAlive = true;
            if (isBoss)
            {
                SetBoss();
            }
        }

        public void Update()
        {
            if (_isMonkeyMove)
            {
                // Debug.Log("VAR");
                transform.Translate(-0.4f * Time.deltaTime, 0, 0);
            }
        }

        private void SetBoss()
        {
            isBossMonster = true;
            Vector3 scale = transform.localScale;
            _tween = transform.DOScale(new Vector3(scale.x * 3f, scale.y * 3f, scale.z), 0.5f).SetId(tweenId);
            StartCoroutine(BossStart(2f));
        }

        IEnumerator BossStart(float time)
        {
            yield return new WaitForSeconds(time);
            ActivateMovement();
        }

        public void HitByProjectile(int damageProj)
        {
            currentHp -= damageProj;
            gameManager.EnemyGetHit();
            DamageText tmpDamage = objectManager.MakeObject("damage", transform.position).GetComponent<DamageText>();
            tmpDamage.SetDamage(damageProj);
            if (currentHp <= 0 && _isAlive)
            {
                currentHp = 0;
                gameManager.GetEnemyXp(isBossMonster);
                _isAlive = false;
                GetComponent<AudioSource>().Play();
                Invoke("DisableObject", 0.2f);
            }
            hpBar.fillAmount = (float)currentHp / totalHp;
        }

        public void DisableObject()
        {
            StopAllCoroutines();
            DOTween.Kill(this);
            gameManager.EnemyDead();
            if (isBossMonster)
            {
                isBossMonster = false;
                Vector3 scale = transform.localScale;
                transform.localScale = new Vector3(scale.x / 3f, scale.y / 3f, scale.z);
            }

            if (dropItem)
            {
                Vector3 tmpVector = hammerItem.transform.position;
                tmpVector.Set(transform.position.x, tmpVector.y, tmpVector.z);
                hammerItem.SetActive(true);
                dropItem = false;
            }
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            DOTween.Kill(this);
            if (isBossMonster)
            {
                isBossMonster = false;
                Vector3 scale = transform.localScale;
                transform.localScale = new Vector3(scale.x / 3f, scale.y / 3f, scale.z);
            }
        }

        public void ActivateMovement()
        {
            switch (_name)
            {
                case "monkey(Clone)":
                    StartCoroutine(MonkeyMove());
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

        IEnumerator MonkeyMove()
        {
            yield return new WaitForSeconds(0.9f);
            _isMonkeyMove = true;
        }

        IEnumerator FoxMove()
        {
            yield return new WaitForSeconds(0.9f);
            _animator.SetBool("isMove", true);
            StartCoroutine(FoxStop());
            _tween = transform.DOLocalMoveX(transform.position.x - 1.5f, 1.2f).SetId(tweenId);
        }

        IEnumerator FoxStop()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetBool("isMove", false);
            if (isBossMonster)
            {
                yield return new WaitForSeconds(2.2f);
                Vector3 pos = transform.position;
                transform.position = new Vector3(Random.Range(38.4f, 45.4f), pos.y, pos.z);
            }
            StartCoroutine(FoxMove());
        }

        IEnumerator GoraniToLeft()
        {
            
            yield return new WaitForSeconds(1.2f);
            float distance = 1.5f;
            if (isBossMonster) distance = 3.5f;
            _spriteRenderer.flipX = false;
            _tween = transform.DOLocalMoveX(transform.position.x - distance, 1f).SetId(tweenId);
            StartCoroutine(GoraniToRight());
        }

        IEnumerator GoraniToRight()
        {
            if (isBossMonster)
            {
                yield return new WaitForSeconds(2.5f);
            }
            else
            {
                yield return new WaitForSeconds(1.2f);
            }
            _spriteRenderer.flipX = true;
            float distance = 1.5f;
            if (isBossMonster) distance = 3.5f;
            _tween = transform.DOLocalMoveX(transform.position.x + distance, 1f).SetId(tweenId);
            StartCoroutine(GoraniToLeft());
        }

        IEnumerator PigeonMove()
        {
            _animator.SetBool("isMove", true);
            yield return new WaitForSeconds(1f);
            Vector3 pos = player.transform.position;
            _tween = transform.DOMove(new Vector3(pos.x + 3, pos.y + 2, pos.z), 2).SetId(tweenId);
            StartCoroutine(PigeonStop());
        }

        IEnumerator PigeonStop()
        {
            yield return new WaitForSeconds(2f);
            _animator.SetBool("isMove", false);
            if (isBossMonster)
            {
                yield return new WaitForSeconds(2f);
                StartCoroutine(PigeonMove());
            }
        }

        IEnumerator CatJump()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetBool("isMove", true);
            Vector3 pos = player.transform.position;
            StartCoroutine(CatStop());
            _tween = transform.DOMoveY(2.5f, 1).SetId(tweenId);
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
            Vector3 newPos = new Vector3(Random.Range(pos.x - 3.5f, pos.x + 3.5f), Random.Range(pos.y - 0.2f, pos.y + 0.7f),
                pos.z);
            if (newPos.x > transform.position.x)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }

            _tween = transform.DOMove(newPos, 2).SetId(tweenId);
            yield return new WaitForSeconds(1.9f);
            StartCoroutine(BatMove());
            
        }

        IEnumerator DogThrow()
        {
            yield return new WaitForSeconds(2);
            float distance;
            if (!isBossMonster)
            {
                distance = 3f;
            }
            else
            {
                distance = Random.Range(2f, 8f);
            }

            GameObject poop = objectManager.MakeObject("poop", transform.position);
            Projectile proj = poop.GetComponent<Projectile>();
            proj.PoopThrow(distance);
            proj.damage = (int)(damage / 2);
            StartCoroutine(DogThrow());
        }

        public void PlaySound(string objectName)
        {
            if (objectName == "arrow(Clone)")
            {
                sfxManager.PlaySfx(2);
            }
            else if (objectName == "pyochang(Clone)")
            {
                sfxManager.PlaySfx(3);
            }
            else
            {
                sfxManager.PlaySfx(1);
            }
        }
    }
}

