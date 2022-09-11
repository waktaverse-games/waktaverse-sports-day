using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class EnemyCollision : MonoBehaviour
    {
        private bool _isActivated = false;
        private Enemy _enemy;
        private bool _isBoss = false;
        private void OnEnable()
        {
            _enemy = GetComponentInParent<Enemy>();
            if (transform.position.x < 9.8f || _isBoss)
            {
                _enemy.ActivateMovement();
                _isActivated = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            GameObject gameObj = col.gameObject;
            if (gameObj.CompareTag("Attack"))
            {
                _enemy.HitByProjectile(gameObj.GetComponent<Projectile>().damage);
            }

            if (gameObj.name == "right" && !_isActivated)
            {
                _isActivated = true;
                _enemy.ActivateMovement();
            }

            if (gameObj.name == "left" && _isActivated)
            {
                _isActivated = false;
                _enemy.DisableObject();
            }
        }
    }
}