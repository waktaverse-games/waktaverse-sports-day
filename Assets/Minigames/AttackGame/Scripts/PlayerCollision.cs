using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class PlayerCollision : MonoBehaviour
    {
        private Player _player;

        private void OnEnable()
        {
            _player = GetComponentInParent<Player>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameObject gameObj = col.gameObject;
            if (gameObj.CompareTag("Enemy"))
            {
                _player.HitByEnemy(gameObj.GetComponent<EnemyCollision>().Damage());
            }

            if (gameObj.CompareTag("Ball"))
            {
                _player.HitByEnemy(gameObj.GetComponent<Projectile>().damage);
            }

            if (gameObj.CompareTag("Coin"))
            {
                _player.HitByCoin();
            }
        }
    }
}