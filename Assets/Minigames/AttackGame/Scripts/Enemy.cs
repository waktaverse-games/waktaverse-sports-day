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
        private float _bossMonkeySpeed = -0.4f;
        private AudioSource _audioSource;

        private void OnEnable()
        {
            _name = gameObject.name;
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _isMonkeyMove = false;
            damage = 10;
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = SharedLibs.SoundManager.Instance.SFXVolume;
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
                
                transform.Translate(_bossMonkeySpeed * Time.deltaTime, 0, 0);
                if (transform.position.x < player.transform.position.x && _bossMonkeySpeed < 0 && isBossMonster)
                {
                    _bossMonkeySpeed *= -1;
                    transform.GetComponent<SpriteRenderer>().flipX = true;
                } else if (transform.position.x > player.transform.position.x && _bossMonkeySpeed > 0 && isBossMonster)
                {
                    _bossMonkeySpeed *= -1;
                    transform.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
        }

        private void SetBoss()
        {
            isBossMonster = true;
            Vector3 scale = transform.localScale;
            _tween = transform.DOScale(new Vector3(scale.x * 3f, scale.y * 3f, scale.z), 0.5f).SetId(tweenId);
            StartCoroutine(BossStart(2f));
            // Debug.Log(isBossMonster);
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
                _audioSource.Play();
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
                transform.localScale = new Vector3(1.5f, 1.5f, scale.z);
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
                transform.localScale = new Vector3(1.5f, 1.5f, scale.z);
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
                    StartCoroutine(GoraniToLeft(2f));
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

        IEnumerator GoraniToLeft(float distance)
        {
            
            yield return new WaitForSeconds(distance * 0.5f + 0.2f);
            distance = Random.Range(1.5f, 2.5f);
            if (isBossMonster) distance = Random.Range(8f, 14f);
            _spriteRenderer.flipX = false;
            _tween = transform.DOLocalMoveX(transform.position.x - distance, distance * 0.5f).SetId(tweenId);
            StartCoroutine(GoraniToRight(distance));
        }

        IEnumerator GoraniToRight(float distance)
        {
            if (isBossMonster)
            {
                yield return new WaitForSeconds(distance * 0.5f + 0.7f);
            }
            else
            {
                yield return new WaitForSeconds(distance * 0.5f + 0.2f);
            }
            _spriteRenderer.flipX = true;
            _tween = transform.DOLocalMoveX(transform.position.x + distance, distance * 0.5f).SetId(tweenId);
            StartCoroutine(GoraniToLeft(distance));
        }

        IEnumerator PigeonMove()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetBool("isMove", true);
            Vector3 pos = player.transform.position;
            if (!isBossMonster)
            {
                _tween = transform.DOMove(new Vector3(pos.x + 4f, pos.y + 4, pos.z), 1).SetId(tweenId);
            }
            else
            {
                if (pos.x > transform.position.x)
                {
                    _spriteRenderer.flipX = true;
                }
                else
                {
                    _spriteRenderer.flipX = false;
                }
                _tween = transform.DOMove(new Vector3(pos.x, pos.y + 5, pos.z), 0.5f).SetId(tweenId);
            }
            StartCoroutine(PigeonStop());
        }

        IEnumerator PigeonStop()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetBool("isMove", false);
            if (isBossMonster)
            {
                yield return new WaitForSeconds(1.3f);
                StartCoroutine(PigeonMove());
            }
        }

        IEnumerator CatJump()
        {
            yield return new WaitForSeconds(1f);
            _animator.SetBool("isMove", true);
            Vector3 pos = player.transform.position;
            if (isBossMonster)
            {
                float randNum = Random.Range(35f, 47f);
                _tween = transform.DOJump(new Vector3(randNum, 1.327732f, 0), 3, 1, 2f);

                yield return new WaitForSeconds(1f);
                StartCoroutine(CatStop());
            }
            else
            {
                _tween = transform.DOMoveY(2.5f, 1).SetId(tweenId);
                StartCoroutine(CatStop());
            }
        }

        IEnumerator CatStop()
        {
            yield return new WaitForSeconds(1f);
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

