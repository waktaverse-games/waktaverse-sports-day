using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class ProjectileInfo : MonoBehaviour
    {
        public enum Type { Straight, Guided, Sector, Slash }

        public Type type;
        public float speed, attackSpeed;
        public int damage;

        public Rigidbody2D rigid;
        [SerializeField] private Transform target;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();

            if (type == Type.Straight || type == Type.Slash) rigid.velocity = Vector2.right * speed;
        }

        private void FixedUpdate()
        {
            if (transform.position.x > 10) Destroy(gameObject);

            if (type == Type.Guided)
            {
                float minDistance = 100;
                foreach (EnemyMove enemy in FindObjectsOfType<EnemyMove>())
                {
                    if (Vector2.Distance(transform.position, enemy.transform.position) < minDistance)
                    {
                        target = enemy.transform;
                        minDistance = Vector2.Distance(transform.position, enemy.transform.position);
                    }
                }
                if (target) rigid.velocity = (target.position - transform.position).normalized * speed;
            }
        }
    }
}