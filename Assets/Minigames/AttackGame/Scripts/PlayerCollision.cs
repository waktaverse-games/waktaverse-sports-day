using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class PlayerCollision : MonoBehaviour
    {
        public SFXManager _sfxManager;
        
        private Player _player;

        private void OnEnable()
        {
            _player = GetComponentInParent<Player>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameObject gameObj = col.gameObject;
            string objectName = gameObj.name;
            if (gameObj.CompareTag("Enemy"))
            {
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
            }

            if (gameObj.CompareTag("UpgradeItem"))
            {
                _player.ActivateItem();
                gameObj.SetActive(false);
                _sfxManager.PlaySfx(0);
            }
        }
    }
}