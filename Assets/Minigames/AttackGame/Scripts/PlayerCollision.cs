using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class PlayerCollision : MonoBehaviour
    {
        public SFXManager _sfxManager;
        public GameObject effect;
        public GameObject damageup;
        
        private Player _player;

        private void OnEnable()
        {
            _player = GetComponentInParent<Player>();
        }
        
        void DisableEffect()
        {
            effect.SetActive(false);
        }

        void DisableDamage()
        {
            damageup.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameObject gameObj = col.gameObject;
            string objectName = gameObj.name;
            if (gameObj.CompareTag("Enemy"))
            {
                _sfxManager.PlaySfx(5);
                _player.HitByEnemy(gameObj.GetComponent<EnemyCollision>().Damage());
            }

            if (gameObj.CompareTag("Ball"))
            {
                _player.HitByEnemy(gameObj.GetComponent<Projectile>().damage);
                gameObj.SetActive(false);
            }

            if (gameObj.CompareTag("Coin"))
            {
                _player.HitByCoin();
                gameObj.SetActive(false);
                _sfxManager.PlaySfx(0);
                effect.SetActive(true);
                effect.GetComponent<Animator>().Play("Star");
                Invoke("DisableEffect", 0.3f);
            }

            if (gameObj.CompareTag("UpgradeItem"))
            {
                _player.ActivateItem();
                gameObj.SetActive(false);
                _sfxManager.PlaySfx(0);
                effect.SetActive(true);
                damageup.SetActive(true);
                effect.GetComponent<Animator>().Play("Star");
                Invoke("DisableEffect", 0.3f);
                Invoke("DisableDamage", 1.2f);
            }
        }
    }
}