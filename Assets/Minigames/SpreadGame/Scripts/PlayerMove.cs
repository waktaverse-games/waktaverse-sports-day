using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace GameHeaven.SpreadGame
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] float speed; // 이동속도

        public int[] bulletLVs; // 현재 보유중인 무기

        [SerializeField] GameObject coinAcquireEffect, bombImageUI;

        [SerializeField] private int bombCnt;
        [SerializeField] private bool hasShield, isStun;

        [SerializeField] AudioClip[] bulletSounds, acquireSounds;
        [SerializeField] AudioClip bombSound;

        [SerializeField] bool isInvincible;

        private int curSectorIdx, curSectorDir;
        PoolManager pool;
        Rigidbody2D rigid;
        Animator anim;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            pool = FindObjectOfType<PoolManager>();
            anim = GetComponent<Animator>();

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
            Invoke("BulletSound", 0);
        }

        void BulletSound()
        {
            AudioSource.PlayClipAtPoint(bulletSounds[0], Vector3.zero);
            Invoke("BulletSound", 0.2f);
        }
        private void Update()
        {
            Move();
            Bomb();

            for (int i = 0; i < 4; i++) // 탄환 총 4개
            {
                if (bulletLVs[i] > 0)
                {
                    Fire(i);
                }
            }

            // 무적 키
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isInvincible)
                {
                    isInvincible = false;
                    GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(false);
                }
                else
                {
                    isInvincible = true;
                    if (!hasShield) hasShield = true;
                    GameObject.Find("Canvas").transform.GetChild(3).gameObject.SetActive(true);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Ball"))
            {
                if (hasShield)
                {
                    StartCoroutine(ShieldBreak());
                }
                else
                {
                    print("GameOver");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            else if (collider.CompareTag("Coin"))
            {
                AudioSource.PlayClipAtPoint(acquireSounds[0], Vector3.zero);
                // coin count up
                Instantiate(coinAcquireEffect, collider.transform.position, coinAcquireEffect.transform.rotation);
                Destroy(collider.gameObject);
            }
            else if (collider.CompareTag("UpgradeItem"))
            {
                AudioSource.PlayClipAtPoint(acquireSounds[1], Vector3.zero);
                switch (collider.name[1])
                {
                    case 'u': // G'u'ided
                        {
                            /*
                            int cnt = 0;
                            for (int i = 0; i < bulletLVs.Length; i++)
                            {
                                if (i == 0) continue;
                                if (bulletLVs[i] > 0) cnt++;
                            }*/

                            if (bulletLVs[0] < 6)
                            {
                                bulletLVs[0]++;

                                BulletInfo bullet = pool.bulletPrefabs[0].GetComponent<BulletInfo>();
                                if (bulletLVs[0] == 3 || bulletLVs[0] == 6) bullet.damage++;
                                bullet.maxShotDelay -= 0.5f;
                            }

                            GameObject obj = GameObject.Find("BulletLV");
                            obj.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255);
                            obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                            obj.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Lv." + bulletLVs[0];
                            break;
                        }
                    case 'e': // S'e'ctor
                        {
                            if (bulletLVs[1] < 6)
                            {
                                bulletLVs[1]++;

                                BulletInfo bullet = pool.bulletPrefabs[1].GetComponent<BulletInfo>();
                                if (bulletLVs[1] == 6) bullet.damage++;
                                bullet.maxShotDelay -= 0.04f;
                            }

                            GameObject obj = GameObject.Find("BulletLV");
                            obj.transform.GetChild(1).GetComponent<Image>().color = new Color(255, 255, 255);
                            obj.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                            obj.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Lv." + bulletLVs[1];
                            break;
                        }
                    case 'l': // S'l'ash
                        {
                            if (bulletLVs[2] < 6)
                            {
                                bulletLVs[2]++;

                                BulletInfo bullet = pool.bulletPrefabs[2].GetComponent<BulletInfo>();
                                if (bulletLVs[2] == 3 || bulletLVs[2] == 6) bullet.damage++;
                                bullet.maxShotDelay -= 0.4f;
                            }

                            GameObject obj = GameObject.Find("BulletLV");
                            obj.transform.GetChild(2).GetComponent<Image>().color = new Color(255, 255, 255);
                            obj.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                            obj.transform.GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Lv." + bulletLVs[2];
                            break;
                        }
                    case 't': // S't'raight
                        {
                            if (bulletLVs[3] < 6)
                            {
                                bulletLVs[3]++;

                                BulletInfo bullet = pool.bulletPrefabs[3].GetComponent<BulletInfo>();
                                if (bulletLVs[3] == 3 || bulletLVs[3] == 6) bullet.damage++;
                                bullet.maxShotDelay -= 0.04f;
                            }

                            GameObject obj = GameObject.Find("BulletLV");
                            obj.transform.GetChild(3).GetComponent<Image>().color = new Color(255, 255, 255);
                            obj.transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
                            obj.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Lv." + bulletLVs[3];
                            break;
                        }

                    case 'o': // B'o'mb
                        {
                            if (bombCnt < 5)
                            {
                                GameObject.Find("Canvas").transform.GetChild(0).GetChild(bombCnt).gameObject.SetActive(true);
                                bombCnt++;
                            }
                            break;
                        }
                    case 'h': // S'h'ield
                        {
                            hasShield = true;
                            transform.GetChild(0).gameObject.SetActive(true);
                            break;
                        }
                }
                Instantiate(coinAcquireEffect, collider.transform.position, coinAcquireEffect.transform.rotation);
                Destroy(collider.gameObject);
            }
            else if (collider.CompareTag("Brick")) // 뚤기 새장
            {
                StartCoroutine(Stun(0.5f, collider));
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Enemy"))
            {
                if (hasShield)
                {
                    StartCoroutine(ShieldBreak());
                }
                else
                {
                    print("GameOver");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
        }

        IEnumerator Stun(float sec, Collider2D col)
        {
            isStun = true;
            rigid.velocity = Vector2.zero;
            col.transform.position = transform.position;
            col.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            yield return new WaitForSeconds(sec);

            isStun = false;
            col.gameObject.SetActive(false);
        }
        void Move()
        {
            if (isStun) return;

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

            if (idx == 2) // slash sound
            {
                AudioSource.PlayClipAtPoint(bulletSounds[1], Vector3.zero);
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

        void Bomb()
        {
            if (!Input.GetKeyDown(KeyCode.Space) || bombCnt <= 0) return;

            AudioSource.PlayClipAtPoint(bombSound, Vector3.zero);

            foreach (EnemyMove enemy in FindObjectsOfType<EnemyMove>())
            {
                enemy.HP -= 30;
            }

            CameraShake();
            
            GameObject.Find("Canvas").transform.GetChild(0).GetChild(bombCnt - 1).gameObject.SetActive(false);
            bombCnt--;
        }

        void CameraShake()
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>()
                .SetTrigger("Shake");
        }

        IEnumerator ShieldBreak()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            anim.SetTrigger("Hit");

            yield return new WaitForSeconds(1);
            if (!isInvincible) hasShield = false;
        }
    }
}