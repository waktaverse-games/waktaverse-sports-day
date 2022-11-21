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

        public bool[] curBullets; // 현재 보유중인 무기

        [SerializeField] GameObject coinAcquireEffect, bombImageUI, canvas;

        [SerializeField] private int bombCnt;
        [SerializeField] private bool hasShield, isStun;

        [SerializeField] AudioClip[] bulletSounds, acquireSounds;
        [SerializeField] AudioClip bombSound;

        [SerializeField] bool isInvincible;

        private int curSectorIdx, curSectorDir;
        PoolManager pool;
        Rigidbody2D rigid;
        Animator anim;
        ScoreUpdate score;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
            pool = FindObjectOfType<PoolManager>();
            anim = GetComponent<Animator>();
            canvas = GameObject.Find("Canvas");
            score = FindObjectOfType<ScoreUpdate>();

            pool.bulletPrefabs[3].GetComponent<BulletInfo>().maxShotDelay = 0.4f;
            /*
            // bullet 초기화
            BulletInfo bullet = pool.bulletPrefabs[0].GetComponent<BulletInfo>();
            bullet.damage = 3; bullet.maxShotDelay = 3.5f;
            bullet = pool.bulletPrefabs[1].GetComponent<BulletInfo>();
            bullet.damage = 1; bullet.maxShotDelay = 0.25f;
            bullet = pool.bulletPrefabs[2].GetComponent<BulletInfo>();
            bullet.damage = 1; bullet.maxShotDelay = 3.0f;
            bullet = pool.bulletPrefabs[3].GetComponent<BulletInfo>();
            bullet.damage = 1; bullet.maxShotDelay = 0.3f;
            */
            curSectorDir = 1;
        }

        private void Update()
        {
            Move();
            Bomb();

            for (int i = 0; i < 4; i++)
            {
                if(curBullets[i]) Fire(i);

                if (i == 3) continue;

                if (canvas.transform.GetChild(2 + i).GetComponent<Image>().fillAmount > 0)
                {
                    canvas.transform.GetChild(2 + i).GetComponent<Image>().fillAmount -= Time.deltaTime * 0.03f;
                }
                else
                {
                    canvas.transform.GetChild(2 + i).GetComponent<Image>().fillAmount = 1;
                    canvas.transform.GetChild(2 + i).gameObject.SetActive(false);
                    curBullets[i] = false;
                }
            }

            /*
            for (int i = 0; i < 4; i++) // 탄환 총 4개
            {
                if (bulletLVs[i)
                {
                    Fire(i);
                }
            }*/

            // 무적 키
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (isInvincible)
                {
                    isInvincible = false;
                    GameObject.Find("Canvas").transform.GetChild(6).gameObject.SetActive(false);
                }
                else
                {
                    isInvincible = true;
                    if (!hasShield) hasShield = true;
                    GameObject.Find("Canvas").transform.GetChild(6).gameObject.SetActive(true);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Ball"))
            {
                if (hasShield)
                {
                    ShieldBreak();
                }
                else
                {
                    StartCoroutine(GameOver());
                }
            }
            else if (collider.CompareTag("Coin"))
            {
                score.score += 30;
                AudioSource.PlayClipAtPoint(acquireSounds[0], Vector3.zero);
                // coin count up
                Instantiate(coinAcquireEffect, collider.transform.position, coinAcquireEffect.transform.rotation);
                Destroy(collider.gameObject);
            }
            else if (collider.CompareTag("UpgradeItem"))
            {
                score.score += 30;
                AudioSource.PlayClipAtPoint(acquireSounds[1], Vector3.zero);
                AcquireItem(collider.gameObject);
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
                    ShieldBreak();
                }
                else
                {
                    StartCoroutine(GameOver());
                }
            }
        }
        

        IEnumerator GameOver()
        {
            Time.timeScale = 0;
            GameObject.Find("Canvas").transform.GetChild(7).gameObject.SetActive(true);

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R));
            Time.timeScale = 1;
            GameObject.Find("Canvas").transform.GetChild(7).gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        void AcquireItem(GameObject obj)
        {
            int grade = 0;

            switch (obj.name[1])
            {
                case 'u': // G'u'ided
                    {
                        curBullets[0] = true;
                        BulletInfo bullet = pool.bulletPrefabs[0].GetComponent<BulletInfo>();
                        grade = obj.transform.GetChild(0).GetComponent<TextMeshPro>().text[1] - 48;

                        switch (grade)
                        {
                            case 1:
                                bullet.damage = 3;
                                bullet.maxShotDelay = 1.3f;
                                break;
                            case 2:
                                bullet.damage = 3;
                                bullet.maxShotDelay = 1.0f;
                                break;
                            case 3:
                                bullet.damage = 3;
                                bullet.maxShotDelay = 0.7f;
                                break;
                            case 4:
                                bullet.damage = 3;
                                bullet.maxShotDelay = 0.4f;
                                break;
                        }
                        canvas.transform.GetChild(2).gameObject.SetActive(true);
                        canvas.transform.GetChild(2).GetComponent<Image>().fillAmount = 1;
                        canvas.transform.GetChild(2).GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
                        canvas.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + grade;
                        break;
                    }
                case 'e': // S'e'ctor
                    {
                        curBullets[1] = true;
                        BulletInfo bullet = pool.bulletPrefabs[1].GetComponent<BulletInfo>();
                        grade = obj.transform.GetChild(0).GetComponent<TextMeshPro>().text[1] - 48;

                        switch (grade)
                        {
                            case 1:
                                bullet.damage = 1;
                                bullet.maxShotDelay = 0.2f;
                                break;
                            case 2:
                                bullet.damage = 1;
                                bullet.maxShotDelay = 0.15f;
                                break;
                            case 3:
                                bullet.damage = 1;
                                bullet.maxShotDelay = 0.1f;
                                break;
                            case 4:
                                bullet.damage = 1;
                                bullet.maxShotDelay = 0.05f;
                                break;
                        }
                        canvas.transform.GetChild(3).gameObject.SetActive(true);
                        canvas.transform.GetChild(3).GetComponent<Image>().fillAmount = 1;
                        canvas.transform.GetChild(3).GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
                        canvas.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + grade;
                        break;
                    }
                case 'l': // S'l'ash
                    {
                        curBullets[2] = true;
                        BulletInfo bullet = pool.bulletPrefabs[2].GetComponent<BulletInfo>();
                        grade = obj.transform.GetChild(0).GetComponent<TextMeshPro>().text[1] - 48;

                        switch (grade)
                        {
                            case 1:
                                bullet.damage = 1;
                                bullet.maxShotDelay = 0.8f;
                                break;
                            case 2:
                                bullet.damage = 1;
                                bullet.maxShotDelay = 0.6f;
                                break;
                            case 3:
                                bullet.damage = 1;
                                bullet.maxShotDelay = 0.4f;
                                break;
                            case 4:
                                bullet.damage = 1;
                                bullet.maxShotDelay = 0.2f;
                                break;
                        }
                        canvas.transform.GetChild(4).gameObject.SetActive(true);
                        canvas.transform.GetChild(4).GetComponent<Image>().fillAmount = 1;
                        canvas.transform.GetChild(4).GetComponent<Image>().sprite = obj.GetComponent<SpriteRenderer>().sprite;
                        canvas.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + grade;
                        break;
                    }
                case 't': // S't'raight
                    {
                        curBullets[3] = true;
                        break;
                    }

                case 'o': // B'o'mb
                    {
                        if (bombCnt < 3)
                        {
                            canvas.transform.GetChild(0).GetChild(bombCnt).gameObject.SetActive(true);
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

            if (grade > 0)
            {
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
            else
            {
                AudioSource.PlayClipAtPoint(bulletSounds[0], Vector3.zero);
            }

            if (idx == 1) // Sector
            {
                pool.MyInstantiate(idx, transform.position).GetComponent<Rigidbody2D>()
                    .velocity = Quaternion.AngleAxis(-5 * curSectorIdx, Vector3.forward) 
                                * new Vector2(0.9848f, 0.1736f) * bullet.speed;

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

            foreach (GameObject enemyBullet in GameObject.FindGameObjectsWithTag("Ball"))
            {
                enemyBullet.SetActive(false);
                Instantiate(coinAcquireEffect, enemyBullet.transform.position, coinAcquireEffect.transform.rotation);

                if (enemyBullet.layer == LayerMask.NameToLayer("Ball")) // 보스패턴
                {
                    StartCoroutine(BakZwiSetActive(enemyBullet));
                }
            }

            CameraShake();
            
            GameObject.Find("Canvas").transform.GetChild(0).GetChild(bombCnt - 1).gameObject.SetActive(false);
            bombCnt--;
        }

        IEnumerator BakZwiSetActive(GameObject obj)
        {
            yield return new WaitForSeconds(1.0f);

            obj.SetActive(true);

        }

        void CameraShake()
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>()
                .SetTrigger("Shake");
        }

        void ShieldBreak()
        {
            if (!isInvincible) hasShield = false;

            transform.GetChild(0).gameObject.SetActive(false);
            anim.SetTrigger("Hit");
        }
    }
}