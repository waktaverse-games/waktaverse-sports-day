using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class BossMove : MonoBehaviour
    {
        public enum Type { RaNi, DdulGi }
        public int HP;

        [SerializeField] Type type;

        private Animator anim;

        [SerializeField] int numOfPatterns;

        [SerializeField] float curPatternDelay, maxPatternDelay;
        [SerializeField] float curStraightFireDelay, maxStraightFireDelay, straightFireSpeed;
        [SerializeField] float curCircleFireDelay, maxCircleFireDelay, circleFireSpeed;
        [SerializeField] Vector2 straightFireDir;
        [SerializeField] float randomDirY;
        [SerializeField] bool isRandomDir;
        [SerializeField] Vector2 circleFireDir, circleFirePivot;
        [SerializeField] int circleFireNum;

        [SerializeField] private GameObject[] coins, upgradeItems, otherItems;
        [SerializeField] private GameObject dieEffect;
        [SerializeField] bool isDeath;

        private float curCageFireDelay;

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

            if (type == Type.DdulGi)
            {
                FireCage();
            }
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

            pool.MyInstantiate(4, transform.position).GetComponent<Rigidbody2D>().velocity =
                (isRandomDir ? new Vector2(Random.Range(0, -1f), randomDirY).normalized : straightFireDir.normalized) * straightFireSpeed;

            curStraightFireDelay = 0;
        }
        void CircleFire()
        {
            curCircleFireDelay += Time.deltaTime;

            if (curCircleFireDelay < maxCircleFireDelay) return;

            for (int i = 0; i < circleFireNum; i++)
            {
                pool.MyInstantiate(4, transform.position + (Vector3) circleFirePivot).GetComponent<Rigidbody2D>().velocity
                    = Quaternion.AngleAxis(360 / circleFireNum * i, Vector3.forward) * circleFireDir.normalized * circleFireSpeed;
            }

            curCircleFireDelay = 0;
        }
        void FireCage()
        {
            curCageFireDelay += Time.deltaTime;

            if (curCageFireDelay < 2.1f) return;

            pool.MyInstantiate(6, new Vector3(6.6f, Random.Range(-3.5f, 3.5f), 0)).GetComponent<Rigidbody2D>().velocity
                = new Vector2(-8, 0);

            curCageFireDelay = 0;
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
            obj.transform.position = new Vector2(5, 2);
            obj.GetComponent<UpDownMove>().isBoss = true;
            obj.GetComponent<UpDownMove>().StartCoroutine(obj.GetComponent<UpDownMove>().BossItemMove(2f));

            obj = Instantiate(upgradeItems[Random.Range(0, 2)], transform.position, Quaternion.Euler(Vector3.zero));
            obj.transform.position = new Vector2(5, -2);
            obj.GetComponent<UpDownMove>().isBoss = true;
            obj.GetComponent<UpDownMove>().StartCoroutine(obj.GetComponent<UpDownMove>().BossItemMove(2f));

            Instantiate(dieEffect, transform.position, dieEffect.transform.rotation);

            Destroy(transform.parent.gameObject);
            GameManager spawnManager = FindObjectOfType<GameManager>();
            spawnManager.isBossTime = false;
        }
    }
}
