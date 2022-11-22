using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameHeaven.AttackGame
{
    public class Player : MonoBehaviour
    {
        public ObjectManager objectManager;
        public float speed = 1.0f;
        public float arrowSpeed = 5.0f;
        public float pyochangTime = 0.75f;
        public int currentWeapon = 2;
        public bool[] weaponsPossible;
        public int[] weaponsPower;
        public GameObject[] squareUIs;
        public GameObject[] leftWhip;
        public GameObject[] rightWhip;
        public GameObject rabbit;
        public bool isGamePlaying;
        public GameManager gameManager;
        public GameObject weapon1;
        public GameObject weapon2;
        public GameObject hammerIcon;

        public bool isHeadingRight = true;
        private bool _stopAction = false;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _screenBoundaries;
        private int _combo;
        private int _rabbit;

        void OnEnable()
        {
            currentWeapon = 1;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            weaponsPossible = new bool[3] { true, false, false };
            weaponsPower = new int[3] { 13, 12, 14 };
            _rabbit = 0;
            rabbit.SetActive(false);
            StartShooting(2f);
            isGamePlaying = true;

        }

        private void OnDisable()
        {
            if (!isHeadingRight)
            {
                _spriteRenderer.flipX = true;
                isHeadingRight = true;
                speed *= -1;
            }
            squareUIs[1].SetActive(false);
            squareUIs[2].SetActive(false);
        }

        public void UpgradeWeapons()
        {
            for (int i = 0; i < 3; i++)
            {
                weaponsPower[i] = (int)Math.Truncate((float)(weaponsPower[i]) * 1.1f);
            }
        }
        
        public void ActivateItem()
        {
            UpgradeWeapons();
            hammerIcon.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (isGamePlaying)
            {
                
                ChangeDirection();
                MovePlayer();
                PressWeaponKey();
                ActivateRabbit();
            }
        }

        public void ActivateRabbit()
        {
            if (Input.GetKeyDown(KeyCode.S) && _rabbit < 2)
            {
                _rabbit += 1;
                if (_rabbit == 2)
                {
                    rabbit.SetActive(true);
                }
            }
        }

        public void HitByEnemy(int damage)
        {
            gameManager.PlayerGetHit(damage);
        }

        public void HitByCoin()
        {
            gameManager.GetCoin();
        }

        public void ChangeDirection()
        {
            if (Input.GetKeyDown(KeyCode.Space) || !isGamePlaying)
            {
                if (isHeadingRight)
                {
                    _spriteRenderer.flipX = false;
                    isHeadingRight = false;
                }
                else
                {
                    _spriteRenderer.flipX = true;
                    isHeadingRight = true;
                }
                speed *= -1;
            }
        }

        private void MovePlayer()
        {
            Vector3 pos = UnityEngine.Camera.main.WorldToViewportPoint(transform.position);
            if (pos.x < 0f)
            {
                pos.x = 0f;
                transform.position = UnityEngine.Camera.main.ViewportToWorldPoint(pos);
            }
            else if (pos.x > 1f)
            {
                pos.x = 1f;
                transform.position = UnityEngine.Camera.main.ViewportToWorldPoint(pos);
            }
            else
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }
        }
        
        void PressWeaponKey()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                WeaponChange(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                WeaponChange(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                WeaponChange(3);
            }
        }

        void WeaponChange(int newNum)
        {
            if (newNum == currentWeapon || !weaponsPossible[newNum - 1]) return;
            squareUIs[currentWeapon - 1].GetComponent<Animator>().SetBool("isSelected", false);
            squareUIs[newNum - 1].GetComponent<Animator>().SetBool("isSelected", true);
            currentWeapon = newNum;
            _combo = 0;
        }

        public void WeaponDrop()
        {
            weapon1.SetActive(true);
            weapon2.SetActive(true);
        }

        public void WeaponFree(int num)
        {
            squareUIs[num - 1].SetActive(true);
            weaponsPossible[num - 1] = true;
            weapon1.SetActive(false);
            weapon2.SetActive(false);
            StartCoroutine(gameManager.CoinDrop());
        }

        void StartShooting(float time)
        {
            StartCoroutine(Shoot(time));
        }

        IEnumerator Shoot(float time)
        {
            yield return new WaitForSeconds(time);
            _combo++;
            if (_stopAction) yield break;
            switch (currentWeapon)
            {
                case 1:
                    StartCoroutine(Shoot(0.6f));
                    ShootWhip();
                    break;
                case 2:
                    StartCoroutine(Shoot(1.35f));
                    ShootArrow();
                    break;
                case 3:
                    StartCoroutine(Shoot(0.9f));
                    ShootPyochang();
                    break;
            }
        }

        void ShootWhip()
        {
            if (isHeadingRight)
            {
                if (_combo == 4)
                {
                    rightWhip[1].SetActive(true);
                    rightWhip[1].GetComponent<Projectile>().damage = weaponsPower[0] * 3;
                    StartCoroutine(StopWhip(rightWhip[1]));
                    _combo = 0;
                }
                else
                {
                    rightWhip[0].SetActive(true);
                    rightWhip[0].GetComponent<Projectile>().damage = weaponsPower[0];
                    StartCoroutine(StopWhip(rightWhip[0]));
                }
            }
            else
            {
                if (_combo == 4)
                {
                    leftWhip[1].SetActive(true);
                    leftWhip[1].GetComponent<Projectile>().damage = weaponsPower[0] * 3;
                    StartCoroutine(StopWhip(leftWhip[1]));
                    _combo = 0;
                }
                else
                {
                    leftWhip[0].SetActive(true);
                    leftWhip[0].GetComponent<Projectile>().damage = weaponsPower[0];
                    StartCoroutine(StopWhip(leftWhip[0]));
                }
            }
        }

        IEnumerator StopWhip(GameObject whipObj)
        {
            yield return new WaitForSeconds(0.3f);
            whipObj.SetActive(false);
        }

        void ShootArrow()
        {
            Vector3 pos = transform.position;
            if (_combo == 4)
            {
                float tempFloat = -0.1f;
                for (int i = 0; i < 3; i++)
                {
                    GameObject arr = objectManager.MakeObject("arrow", new Vector3(pos.x + tempFloat, 
                        pos.y + tempFloat, pos.z));
                    Projectile proj = arr.GetComponent<Projectile>();
                    proj.SetState(isHeadingRight);
                    proj.damage = weaponsPower[1];
                    proj.ShootArrow(isHeadingRight, arrowSpeed);
                    tempFloat += 0.1f;
                }

                _combo = 0;
            }
            else
            {
                GameObject arr = objectManager.MakeObject("arrow", pos);
                Projectile proj = arr.GetComponent<Projectile>();
                proj.SetState(isHeadingRight);
                proj.damage = weaponsPower[1];
                proj.ShootArrow(isHeadingRight, arrowSpeed);
            }
        }

        void ShootPyochang()
        {
            Vector3 pos = transform.position;
            if (_combo == 4)
            {
                float tempFloat = 3.5f;
                for (int i = 0; i < 3; i++)
                {
                    GameObject arr = objectManager.MakeObject("pyochang", pos);
                    Projectile proj = arr.GetComponent<Projectile>();
                    proj.SetState(isHeadingRight);
                    proj.damage = weaponsPower[2];
                    proj.ShootPyochang(isHeadingRight, pyochangTime, tempFloat);
                    tempFloat += 0.6f;
                }

                _combo = 0;
            }
            else
            {
                GameObject arr = objectManager.MakeObject("pyochang", pos);
                Projectile proj = arr.GetComponent<Projectile>();
                proj.SetState(isHeadingRight);
                proj.damage = weaponsPower[2];
                proj.ShootPyochang(isHeadingRight, pyochangTime, 4.5f);
            }
        }
    }
}

