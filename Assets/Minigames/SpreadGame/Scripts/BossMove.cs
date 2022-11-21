using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameHeaven.SpreadGame
{
    public class BossMove : MonoBehaviour
    {
        public enum Type { RaNi, DdulGi, DdongGangAji, GyunNyang, JuPokDo }
        public int HP, maxHP;

        [SerializeField] Type type;

        private Animator anim;

        [SerializeField] int numOfPatterns, bulletIdx;

        [SerializeField] float curPatternDelay, maxPatternDelay;
        [SerializeField] float curStraightFireDelay, maxStraightFireDelay, straightFireSpeed;
        [SerializeField] float curCircleFireDelay, maxCircleFireDelay, circleFireSpeed;
        [SerializeField] Vector2 straightFireDir;
        [SerializeField] float randomDirY;
        [SerializeField] bool isRandomDir;
        [SerializeField] Vector2 circleFireDir, circleFirePivot;
        [SerializeField] int circleFireNum;

        [SerializeField] private GameObject[] coins, upgradeItems, otherItems;
        [SerializeField] private GameObject dieEffect, gyunNyang;
        [SerializeField] bool isDeath;
        [SerializeField] Sprite PokJu;

        GameObject HPBar;

        private float curCageFireDelay;

        PoolManager pool;

        private void Awake()
        {
            anim = transform.parent.gameObject.GetComponent<Animator>();
            pool = FindObjectOfType<PoolManager>();
            HPBar = GameObject.Find("Canvas").transform.GetChild(5).gameObject;
            HPBar.SetActive(true);

            bulletIdx = 4;
            if (type == Type.DdongGangAji)
            {
                bulletIdx = 5;
            }
            else if (type == Type.GyunNyang)
            {
                StartCoroutine(SpawnGyunNyang(5.1f));
            }
            else if (type == Type.JuPokDo)
            {
                anim.speed = 1.5f;
            }
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

            HPBar.transform.GetChild(1).GetComponent<Image>().fillAmount = (float)HP / (float)maxHP;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (isDeath) return;
            
            if (collider.CompareTag("Attack"))
            {
                anim.SetTrigger("Hit");
                int prevHP = HP;
                HP -= collider.GetComponent<BulletInfo>().damage;
                if (collider.GetComponent<BulletInfo>().type != BulletInfo.Type.Slash) collider.gameObject.SetActive(false);

                if (type == Type.DdongGangAji)
                {
                    transform.localScale += transform.localScale * (float)(prevHP-HP)/150f;

                    if (prevHP >= maxHP * 2 / 3 && HP < maxHP * 2 / 3)
                    {
                        StartCoroutine(PoopRain(6.0f, 60));
                        return;
                    }
                    else if (prevHP >= maxHP * 1 / 3 && HP < maxHP * 1 / 3)
                    {
                        StartCoroutine(PoopRain(6.0f, 80));
                        return;
                    }
                    else if (prevHP >= maxHP / 5 && HP < maxHP / 5)
                    {
                        StartCoroutine(PoopRain(6.0f, 100));
                        return;
                    }
                }
                else if (type == Type.JuPokDo)
                {
                    if (prevHP >= maxHP / 5 && HP < maxHP / 5)
                    {
                        GetComponent<Animator>().enabled = false;
                        anim.SetTrigger("PokJu");
                        GetComponent<SpriteRenderer>().color = Color.red;
                        anim.speed = 1.0f;
                        maxPatternDelay = 2.5f;
                        return;
                    }
                }

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

            pool.MyInstantiate(bulletIdx, transform.position).GetComponent<Rigidbody2D>().velocity =
                (isRandomDir ? new Vector2(Random.Range(0, -1f), randomDirY).normalized : straightFireDir.normalized) * straightFireSpeed;

            curStraightFireDelay = 0;
        }
        void CircleFire()
        {
            curCircleFireDelay += Time.deltaTime;

            if (curCircleFireDelay < maxCircleFireDelay) return;

            for (int i = 0; i < circleFireNum; i++)
            {
                pool.MyInstantiate(bulletIdx, transform.position + (Vector3) circleFirePivot).GetComponent<Rigidbody2D>().velocity
                    = Quaternion.AngleAxis(360 / circleFireNum * i, Vector3.forward) * circleFireDir.normalized * circleFireSpeed;
            }

            curCircleFireDelay = 0;
        }
        void FireCage()
        {
            curCageFireDelay += Time.deltaTime;

            if (curCageFireDelay < 1.4f) return;

            pool.MyInstantiate(6, new Vector3(6.6f, Random.Range(-3.5f, 3.5f), 0)).GetComponent<Rigidbody2D>().velocity
                = new Vector2(-8, 0);

            curCageFireDelay = 0;
        }
        IEnumerator PoopRain(float sec, int num)
        {
            WaitForSeconds wait = new WaitForSeconds(sec / (float)num);

            curPatternDelay = 0;
            anim.SetTrigger("Pattern1");

            yield return new WaitForSeconds(1.0f);

            transform.localScale = new Vector3(2.7f, 2.55f, 1);

            for (int i = 0; i < num; i++)
            {
                pool.MyInstantiate(bulletIdx, new Vector2(Random.Range(-7f, 7f), 3.8f)).GetComponent<Rigidbody2D>().velocity = Vector2.down * 6;

                yield return wait;
            }
        }

        IEnumerator SpawnGyunNyang(float sec)
        {
            WaitForSeconds wait = new WaitForSeconds(sec);

            while (true)
            {
                yield return wait;

                Instantiate(gyunNyang, transform.position, transform.rotation).GetComponent<EnemyMove>().HP = maxHP * 2 / 100;
            }
        }

        IEnumerator Die()
        {
            FindObjectOfType<ScoreUpdate>().score += 500;
            if (type == Type.GyunNyang)
            {
                foreach (EnemyMove enemy in FindObjectsOfType<EnemyMove>())
                {
                    enemy.Die();
                }
            }

            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.maxNormalMonsterSpawnDelay -= 0.3f;
            if (gameManager.maxNormalMonsterSpawnDelay < 0.7f) gameManager.maxNormalMonsterSpawnDelay = 0.7f;
            gameManager.bossIdx++;

            pool.bulletPrefabs[3].GetComponent<BulletInfo>().maxShotDelay -= 0.03f;
            if (pool.bulletPrefabs[3].GetComponent<BulletInfo>().maxShotDelay < 0.12f) pool.bulletPrefabs[3].GetComponent<BulletInfo>().maxShotDelay = 0.12f;
            isDeath = true;
            HPBar.SetActive(false);

            GameObject obj = null;

            WaitForSeconds wait = new WaitForSeconds(0.2f);

            for (int i = 0; i < 20; i++)
            {
                Instantiate(dieEffect, new Vector3(5 + Random.Range(-2f, 2f), Random.Range(-3f, 3f)),
                    dieEffect.transform.rotation).transform.localScale = new Vector3(1.0f, 1.0f);

                Instantiate(coins[Random.Range(0, 3)], new Vector3(9, Random.Range(-4.0f, 4.0f), 0), Quaternion.Euler(Vector3.zero));

                yield return wait;
            }

            int ran1 = Random.Range(0, 4), ran2 = Random.Range(0, 4);

            while(ran1 == ran2)
            {
                ran2 = Random.Range(0, 4);
            }

            /*
            obj = Instantiate(upgradeItems[Random.Range(0, 3)], transform.position, Quaternion.Euler(Vector3.zero));
            obj.transform.GetChild(0).GetComponent<TextMeshPro>().text =
                "x" + (FindObjectOfType<GameManager>().bossIdx / 3 + 1);
            obj.transform.position = new Vector2(5, 2);
            obj.GetComponent<UpDownMove>().isBoss = true;
            obj.GetComponent<UpDownMove>().StartCoroutine(obj.GetComponent<UpDownMove>().BossItemMove(0.5f));

            obj = Instantiate(upgradeItems[Random.Range(0, 3)], transform.position, Quaternion.Euler(Vector3.zero));
            obj.transform.GetChild(0).GetComponent<TextMeshPro>().text =
                "x" + (FindObjectOfType<GameManager>().bossIdx / 3 + 1);
            obj.transform.position = new Vector2(5, -2);
            obj.GetComponent<UpDownMove>().isBoss = true;
            obj.GetComponent<UpDownMove>().StartCoroutine(obj.GetComponent<UpDownMove>().BossItemMove(0.5f));
            */
            GameManager spawnManager = FindObjectOfType<GameManager>();
            spawnManager.isBossTime = false;


            Destroy(transform.parent.gameObject);
        }
    }
}
