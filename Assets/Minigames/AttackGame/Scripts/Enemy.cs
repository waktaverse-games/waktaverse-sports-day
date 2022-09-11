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
        // Start is called before the first frame update
        void OnEnable()
        {
            hpBar.fillAmount = 0.5f;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void HitByProjectile()
        {
            hpBar.fillAmount = (float)currentHp / totalHp;
        }
    }
}

