using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class GuidedMove : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rigid;
        [SerializeField] private Transform target; // target == null이면 가장 가까운 적
        [SerializeField] private float speed;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (target == null)
            {
                float minDistance = 100;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (Vector2.Distance(transform.position, enemy.transform.position) < minDistance)
                    {
                        target = enemy.transform;
                        minDistance = Vector2.Distance(transform.position, enemy.transform.position);
                    }
                }
            }

            if (target)
            {
                rigid.AddForce((target.position - transform.position).normalized, ForceMode2D.Impulse);
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.Euler(0, 0, -90 + Vector2.SignedAngle(Vector2.right, target.position - transform.position)),
                    Time.fixedDeltaTime * 5);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
                rigid.AddForce(Vector2.right, ForceMode2D.Impulse);
            }

            if (rigid.velocity.sqrMagnitude > speed * speed) rigid.velocity = rigid.velocity.normalized * speed;
        }
    }
}
