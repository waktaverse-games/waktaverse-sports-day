using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class EnemyMove : MonoBehaviour
    {
        public enum Type { BakZwi, DdongGae, DdulGi, Fox, PanZee, RaNi, SeGyun }
        public float speed;
        [SerializeField] private float thinkingSpeed, attackSpeed, projectileSpeed;
        [SerializeField] private int HP;
        [SerializeField] private GameObject projectile;
        [SerializeField] private Type type;

        GameObject player;

        Rigidbody2D rigid;
        Animator anim;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            anim = GetComponent<Animator>();

            if (type != Type.BakZwi) StartCoroutine(RandomMove(thinkingSpeed));
            else rigid.velocity = Vector2.left * 10;

            if (type == Type.PanZee) StartCoroutine(Project(projectile));
        }

        private void Update()
        {
            if (HP <= 0) Die();

            if (transform.position.y < -4 || transform.position.y > 4)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, -rigid.velocity.y);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Attack"))
            {
                anim.SetTrigger("Hit");
                HP -= collider.GetComponent<ProjectileInfo>().damage;
                if(collider.GetComponent<ProjectileInfo>().type != ProjectileInfo.Type.Slash)Destroy(collider.gameObject);
            }
        }

        IEnumerator RandomMove(float sec)
        {
            while (true)
            {
                if (type == Type.DdulGi && Vector2.Distance(player.transform.position, transform.position) < 5
                    && player.transform.position.x < transform.position.x)
                {
                    anim.SetTrigger("Rush");
                    rigid.velocity = Vector2.zero;
                    Invoke("Rush", 0.5f);
                }
                else if (type == Type.RaNi)
                {
                    rigid.velocity = new Vector2(-speed, Random.Range(-1, 2) * 3);
                }
                else
                {
                    rigid.velocity = new Vector2(-speed, Random.Range(-1, 2));
                }
                yield return new WaitForSeconds(sec);
            }
        }

        void Rush()
        {
            rigid.velocity = (player.transform.position - transform.position).normalized * 5;
        }

        IEnumerator Project(GameObject projectile)
        {
            while (true)
            {
                GameObject obj = Instantiate(projectile, transform.position, transform.rotation);
                obj.GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * projectileSpeed;
                yield return new WaitForSeconds(attackSpeed);
            }
        }

        void Die()
        {
            // Animation
            Destroy(gameObject);
        }
    }
}