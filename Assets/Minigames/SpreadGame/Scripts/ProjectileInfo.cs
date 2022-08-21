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
            if (transform.position.x > 7) Destroy(gameObject);

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
                if (target)
                {
                    rigid.AddForce((target.position - transform.position).normalized, ForceMode2D.Impulse);
                    print(Vector2.Angle(transform.position, target.position));
                    transform.rotation = Quaternion.Lerp(transform.rotation,
                        Quaternion.Euler(0, 0, -90 + Vector2.SignedAngle(Vector2.right, target.position - transform.position)),
                        Time.fixedDeltaTime * 5);
                }

                if (rigid.velocity.sqrMagnitude > speed * speed) rigid.velocity = rigid.velocity.normalized * speed;
            }
        }
    }
}