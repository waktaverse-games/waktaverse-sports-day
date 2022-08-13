using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class EnemyMove : MonoBehaviour
    {
        [SerializeField] float speed, thinkingSpeed, attackSpeed, projectileSpeed;
        [SerializeField] int HP;
        [SerializeField] GameObject projectile;

        GameObject player;

        Rigidbody2D rigid;
        Animator anim;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            anim = GetComponent<Animator>();

            StartCoroutine(RandomMove(thinkingSpeed));
            StartCoroutine(Project(projectile));
        }

        private void Update()
        {
            if (HP <= 0) Die();

            transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -4, 4));
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Attack"))
            {
                anim.SetTrigger("Hit");
                HP -= collider.GetComponent<ProjectileInfo>().damage;
                Destroy(collider.gameObject);
            }
        }

        IEnumerator RandomMove(float sec)
        {
            rigid.velocity = new Vector2(-1, Random.Range(-1, 2)).normalized * speed;

            yield return new WaitForSeconds(sec);
            StartCoroutine(RandomMove(sec));
        }

        IEnumerator Project(GameObject projectile)
        {
            GameObject obj = Instantiate(projectile, transform.position, transform.rotation);

            obj.GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * projectileSpeed;

            yield return new WaitForSeconds(attackSpeed);
            StartCoroutine(Project(projectile));
        }

        void Die()
        {
            // Animation
            Destroy(gameObject);
        }
    }
}