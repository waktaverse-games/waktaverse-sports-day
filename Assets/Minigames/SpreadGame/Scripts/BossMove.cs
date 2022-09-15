using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class BossMove : MonoBehaviour
    {
        public enum Type { RaNi }
        public int HP;

        [SerializeField] Type type;

        private Animator anim;

        [SerializeField] int numOfPatterns;

        [SerializeField] float curPatternDelay, maxPatternDelay;
        [SerializeField] float curStraightFireDelay, maxStraightFireDelay, straightFireSpeed;
        [SerializeField] float curCircleFireDelay, maxCircleFireDelay, circleFireSpeed;
        [SerializeField] Vector2 straightFireDir;
        [SerializeField] int circleFireNum;

        [SerializeField] private GameObject[] coins, upgradeItems, otherItems;
        [SerializeField] private GameObject dieEffect;
        [SerializeField] bool isDeath;

        PoolManager pool;

        private void Awake()
        {
            anim = transform.parent.gameObject.GetComponent<Animator>();
            pool = FindObjectOfType<PoolManager>();
        }

        private void Update()
        {
            if (isDeath) return;
            Move();
            Fire();
            CircleFire();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (isDeath) return;

            if (collider.CompareTag("Attack"))
            {
                anim.SetTrigger("Hit");
                HP -= collider.GetComponent<BulletInfo>().damage;
                if (collider.GetComponent<BulletInfo>().type != BulletInfo.Type.Slash) collider.gameObject.SetActive(false);

                if (HP <= 0) StartCoroutine(Die());
            }
        }

        void Move()
        {
            curPatternDelay += Time.deltaTime;

            if (curPatternDelay < maxPatternDelay) return;

            int random = Random.Range(0, numOfPatterns);

            anim.SetTrigger("Pattern" + random.ToString());

            curPatternDelay = 0;
        }
        void Fire()
        {
            curStraightFireDelay += Time.deltaTime;

            if (curStraightFireDelay < maxStraightFireDelay) return;

            pool.MyInstantiate(4, transform.position).GetComponent<Rigidbody2D>().velocity = straightFireDir * straightFireSpeed;

            curStraightFireDelay = 0;
        }
        void CircleFire()
        {
            curCircleFireDelay += Time.deltaTime;

            if (curCircleFireDelay < maxCircleFireDelay) return;

            for (int i = 0; i < circleFireNum; i++)
            {
                pool.MyInstantiate(4, transform.position).GetComponent<Rigidbody2D>().velocity
                    = Quaternion.AngleAxis(360 / circleFireNum * i, Vector3.forward) * Vector2.right * circleFireSpeed;
            }

            curCircleFireDelay = 0;
        }

        IEnumerator Die()
        {
            isDeath = true;

            GameObject obj = null;

            int[] bulletLVs = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().bulletLVs;

            int cnt = 0;
            for (int i = 0; i < bulletLVs.Length; i++)
            {
                if (bulletLVs[i] > 0) cnt++;
            }

            WaitForSeconds wait = new WaitForSeconds(0.2f);

            for (int i = 0; i < 20; i++)
            {
                Instantiate(dieEffect, transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)),
                    dieEffect.transform.rotation).transform.localScale = new Vector3(1.0f, 1.0f);

                Instantiate(coins[Random.Range(0, 3)], new Vector3(9, Random.Range(-4.0f, 4.0f), 0), Quaternion.Euler(Vector3.zero));

                yield return wait;
            }

            obj = Instantiate(upgradeItems[Random.Range(0, 4)], transform.position, Quaternion.Euler(Vector3.zero));
            obj.transform.localScale = new Vector3(0.5f, 0.5f);

            obj = Instantiate(otherItems[Random.Range(0, 2)], transform.position, Quaternion.Euler(Vector3.zero));
            obj.GetComponent<UpDownMove>().dir = new Vector3(0, -0.05f, 0);
            obj.transform.localScale = new Vector3(0.5f, 0.5f);

            Instantiate(dieEffect, transform.position, dieEffect.transform.rotation);

            Destroy(transform.parent.gameObject);
            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            spawnManager.isBossTime = false;
        }
    }
}
