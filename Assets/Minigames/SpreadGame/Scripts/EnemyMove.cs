using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GameHeaven.SpreadGame
{
    public class EnemyMove : MonoBehaviour
    {
        public enum Type { BakZwi, DdongGangAji, DdulGi, JuPokDo, PanZee, RaNi, SeGyun, GyunNyang }
        public float speed;
        [SerializeField] private float thinkingSpeed, attackSpeed, projectileSpeed;
        public int HP;
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject[] coins, upgradeItems, otherItems;
        [SerializeField] private Type type;
        [SerializeField] private GameObject dieEffect;
        GameObject player;
        public bool isElite;
        public bool isCopy;

        [SerializeField] private AudioClip dieSound;

        public Rigidbody2D rigid;
        Animator anim;

        PoolManager pool;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
            anim = GetComponent<Animator>();

            pool = FindObjectOfType<PoolManager>();

            if (type != Type.BakZwi) StartCoroutine(RandomMove(thinkingSpeed));
            else rigid.velocity = Vector2.left * 10;

            if (type == Type.PanZee || type == Type.DdongGangAji) StartCoroutine(Fire(projectile));

            if (type == Type.SeGyun || type == Type.GyunNyang) Invoke("Reproduction", 3.0f);
        }

        private void Update()
        {
            if (HP <= 0) Die();
            if (transform.position.x < -7) Destroy(gameObject);

            if (type == Type.SeGyun || type == Type.JuPokDo)
            {
                rigid.AddForce(new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * 0.01f, ForceMode2D.Impulse);
            }

            if (transform.position.y < -4)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, (rigid.velocity.y > 0) ? rigid.velocity.y : -rigid.velocity.y);
            }
            else if (transform.position.y > 4)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, (rigid.velocity.y > 0) ? -rigid.velocity.y : rigid.velocity.y);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Attack"))
            {
                anim.SetTrigger("Hit");
                HP -= collider.GetComponent<BulletInfo>().damage;
                if (collider.GetComponent<BulletInfo>().type != BulletInfo.Type.Slash) collider.gameObject.SetActive(false);
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
                else if (type == Type.GyunNyang)
                {
                    rigid.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed;
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
        IEnumerator Fire(GameObject projectile)
        {
            WaitForSeconds wait = new WaitForSeconds(attackSpeed);

            while (true)
            {
                yield return wait;

                GameObject obj = pool.MyInstantiate(4, transform.position);
                obj.GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * projectileSpeed;
            }
        }
        void Reproduction()
        {
            EnemyMove copy = Instantiate(this.gameObject, transform.position, transform.rotation).GetComponent<EnemyMove>();
            copy.isCopy = true;
            if (isElite)
            {
                copy.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else
            {
                copy.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        public void Die()
        {
            AudioSource.PlayClipAtPoint(dieSound, Vector3.zero);
            GameObject obj = null;

            if (Random.Range(0, 3) == 0) Instantiate(coins[Random.Range(0, 3)], transform.position, Quaternion.Euler(Vector3.zero));

            if (isElite && !isCopy)
            {
                if(Random.Range(0,3) == 0)
                {
                    obj = Instantiate(otherItems[Random.Range(0, 2)], transform.position, Quaternion.Euler(Vector3.zero));
                }
                else
                {
                    obj = Instantiate(upgradeItems[Random.Range(0, 3)], transform.position, Quaternion.Euler(Vector3.zero));
                    obj.transform.GetChild(0).GetComponent<TextMeshPro>().text =
                        "x" + Random.Range(Mathf.Min(2, 1 + FindObjectOfType<GameManager>().bossIdx / 4), 
                        Mathf.Min(5, 2 + FindObjectOfType<GameManager>().bossIdx / 2));
                }
                obj.transform.localScale = new Vector3(0.5f, 0.5f);

                if (Random.Range(0, 3) == 0)
                {
                    obj = Instantiate(otherItems[Random.Range(0, 2)], transform.position, Quaternion.Euler(Vector3.zero));
                }
                else
                {
                    obj = Instantiate(upgradeItems[Random.Range(0, 3)], transform.position, Quaternion.Euler(Vector3.zero));
                    obj.transform.GetChild(0).GetComponent<TextMeshPro>().text =
                        "x" + Random.Range(Mathf.Min(3, 1 + FindObjectOfType<GameManager>().bossIdx / 4),
                        Mathf.Min(5, 1 + FindObjectOfType<GameManager>().bossIdx / 2));
                }
                obj.GetComponent<UpDownMove>().dir = new Vector3(0, -0.05f, 0);
                obj.transform.localScale = new Vector3(0.5f, 0.5f);
            }

            Instantiate(dieEffect, transform.position, dieEffect.transform.rotation);
            Destroy(gameObject);
        }
    }
}