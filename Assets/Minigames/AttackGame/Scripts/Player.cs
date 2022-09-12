using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class Player : MonoBehaviour
    {
        public float speed = 1.0f;
        public float arrowSpeed = 5.0f;
        public int currentWeapon = 1;
        public bool[] weaponsPossible;
        public int[] weaponsPower;
        public GameObject[] squareUIs;
        
        private bool _isRight = true;
        private SpriteRenderer _spriteRenderer;
        private Vector2 _screenBoundaries;
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            weaponsPossible = new bool[3] { true, false, false };
            weaponsPower = new int[3] { 9, 10, 8 };
        }

        // Update is called once per frame
        void Update()
        {
            ChangeDirection();
            MovePlayer();
            PressWeaponKey();
        }

        private void ChangeDirection()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_isRight)
                {
                    _spriteRenderer.flipX = false;
                    _isRight = false;
                }
                else
                {
                    _spriteRenderer.flipX = true;
                    _isRight = true;
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
        }

        IEnumerator Shoot(float time)
        {
            yield return new WaitForSeconds(time);
            switch (currentWeapon)
            {
                case 1:
                    ShootWhip();
                    break;
                case 2:
                    ShootArrow();
                    break;
                case 3:
                    ShootPyochang();
                    break;
            }
        }

        void ShootWhip()
        {
            
        }

        void ShootArrow()
        {
            
        }

        void ShootPyochang()
        {
            
        }
    }
}

