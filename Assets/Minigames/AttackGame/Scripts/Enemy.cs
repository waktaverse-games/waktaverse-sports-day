using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        private void Start()
        {
            _name = gameObject.name;
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
            
        }
    }
}

