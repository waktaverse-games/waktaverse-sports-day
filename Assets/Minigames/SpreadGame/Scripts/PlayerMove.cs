using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.SpreadGame
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] float speed; // 이동속도

        public int[] bulletLVs; // 현재 보유중인 무기

        [SerializeField] GameObject coinAcquireEffect;

        [SerializeField] private int curSectorIdx, curSectorDir;

        PoolManager pool;
        Rigidbody2D rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            pool = FindObjectOfType<PoolManager>();

            bulletLVs[0] = bulletLVs[1] = bulletLVs[2] = 0;
            bulletLVs[3] = 1;

            // bullet 초기화
            BulletInfo bullet = pool.bulletPrefabs[0].GetComponent<BulletInfo>();
            bullet.damage = 3; bullet.maxShotDelay = 3.5f;
            bullet = pool.bulletPrefabs[1].GetComponent<BulletInfo>();
            bullet.damage = 1; bullet.maxShotDelay = 0.25f;
            bullet = pool.bulletPrefabs[2].GetComponent<BulletInfo>();
            bullet.damage = 1; bullet.maxShotDelay = 3.0f;
            bullet = pool.bulletPrefabs[3].GetComponent<BulletInfo>();
            bullet.damage = 1; bullet.maxShotDelay = 0.3f;

            curSectorDir = 1;
        }

        private void Update()
        {
            Move();

            for (int i = 0; i < 4; i++) // 탄환 총 4개
            {
                if (bulletLVs[i] > 0)
                {
                    Fire(i);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Enemy"))
            {
                print("GameOver");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (collider.CompareTag("Coin"))
            {
                // coin count up
                Instantiate(coinAcquireEffect, collider.transform.position, coinAcquireEffect.transform.rotation);
                Destroy(collider.gameObject);
            }
            else if (collider.CompareTag("UpgradeItem"))
            {
                print(collider.name[0] + "/" + collider.name[9]);
                switch (collider.name[9])
                {
                    case 'u': // G'u'ided
                        {
                            int cnt = 0;
                            for (int i = 0; i < bulletLVs.Length; i++)
                            {
                                if (i == 0) continue;
                                if (bulletLVs[i] > 0) cnt++;
                            }

                            if (cnt < 3 && bulletLVs[0] < 6)
                            {
                                bulletLVs[0]++;

                                BulletInfo bullet = pool.bulletPrefabs[0].GetComponent<BulletInfo>();
                                if (bulletLVs[0] == 3 || bulletLVs[0] == 6) bullet.damage++;
                                bullet.maxShotDelay -= 0.5f;
                            }
                            break;
                        }
                    case 'e': // S'e'ctor
                        {
                            int cnt = 0;
                            for (int i = 0; i < bulletLVs.Length; i++)
                            {
                                if (i == 1) continue;
                                if (bulletLVs[i] > 0) cnt++;
                            }
                            if (cnt < 3 && bulletLVs[1] < 6)
                            {
                                bulletLVs[1]++;

                                BulletInfo bullet = pool.bulletPrefabs[1].GetComponent<BulletInfo>();
                                if (bulletLVs[1] == 6) bullet.damage++;
                                bullet.maxShotDelay -= 0.04f;
                            }
                            break;
                        }
                    case 'l': // S'l'ash
                        {
                            int cnt = 0;
                            for (int i = 0; i < bulletLVs.Length; i++)
                            {
                                if (i == 2) continue;
                                if (bulletLVs[i] > 0) cnt++;
                            }

                            if (cnt < 3 && bulletLVs[2] < 6)
                            {
                                bulletLVs[2]++;

                                BulletInfo bullet = pool.bulletPrefabs[2].GetComponent<BulletInfo>();
                                if (bulletLVs[2] == 3 || bulletLVs[2] == 6) bullet.damage++;
                                bullet.maxShotDelay -= 0.4f;
                            }
                            break;
                        }
                    case 't': // S't'raight
                        {
                            int cnt  = 0;
                            for (int i = 0; i < bulletLVs.Length; i++)
                            {
                                if (i == 3) continue;
                                if (bulletLVs[i] > 0) cnt++;
                            }

                            if (cnt < 3 && bulletLVs[3] < 6)
                            {
                                bulletLVs[3]++;

                                BulletInfo bullet = pool.bulletPrefabs[3].GetComponent<BulletInfo>();
                                if (bulletLVs[3] == 3 || bulletLVs[3] == 6) bullet.damage++;
                                bullet.maxShotDelay -= 0.04f;
                            }
                            break;
                        }
                }
                foreach (GameObject del in GameObject.FindGameObjectsWithTag("UpgradeItem"))
                {
                    Destroy(del);
                }
            }
        }

        void Move()
        {
            if (rigid.velocity.sqrMagnitude > speed * speed)
            {
                rigid.velocity = rigid.velocity.normalized * speed;
            }
            else
            {
                rigid.velocity *= 0.8f;
            }

            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                rigid.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
            }
            else if (Input.GetAxisRaw("Horizontal") == -1)
            {
                rigid.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
            }

            if (Input.GetAxisRaw("Vertical") == 1)
            {
                rigid.AddForce(Vector2.up * speed, ForceMode2D.Impulse);
            }
            else if (Input.GetAxisRaw("Vertical") == -1)
            {
                rigid.AddForce(Vector2.down * speed, ForceMode2D.Impulse);
            }
        }

        void Fire(int idx)
        {
            BulletInfo bullet = pool.bulletPrefabs[idx].GetComponent<BulletInfo>();

            bullet.curShotDelay += Time.deltaTime; // 장전 시간

            if (bullet.curShotDelay < bullet.maxShotDelay)
            {
                return;
            }

            if (idx == 1) // Sector
            {
                pool.MyInstantiate(idx, transform.position).GetComponent<Rigidbody2D>()
                    .velocity = Quaternion.AngleAxis(-15 * curSectorIdx, Vector3.forward) 
                                * new Vector2(0.866f, 0.5f) * bullet.speed;

                curSectorIdx += curSectorDir;
                if (curSectorIdx >= 4 || curSectorIdx <= 0) curSectorDir *= -1;
            }
            else
            {
                pool.MyInstantiate(idx, transform.position).GetComponent<Rigidbody2D>().AddForce(new Vector2(bullet.speed, 0),ForceMode2D.Impulse);
            }

            bullet.curShotDelay = 0;
        }
    }
}