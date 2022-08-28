using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.SpreadGame
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] float speed;

        [SerializeField] GameObject[] bulletPrefabs;
        [SerializeField] int[] bulletLVs;
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

            curSectorDir = 1;
        }

        private void Update()
        {
            Move();

            for (int i = 0; i < 4; i++) // ≈∫»Ø √— 4∞≥
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
                    /*
                    case 'u': // G'u'ided
                        if (projectilesLV[0] == 0) StartCoroutine(Project(projectiles[0]));
                        if (projectilesLV[0] < 6) projectilesLV[0]++;
                        break;
                    case 'e': // S'e'ctor
                        if (projectilesLV[1] == 0) StartCoroutine(Project(projectiles[1]));
                        if (projectilesLV[1] < 6) projectilesLV[1]++;
                        break;
                    case 'l': // S'l'ash
                        if (projectilesLV[2] == 0) StartCoroutine(Project(projectiles[2]));
                        if (projectilesLV[2] < 6) projectilesLV[2]++;
                        break;
                    case 't': // S't'raight
                        if (projectilesLV[3] < 6) projectilesLV[3]++;
                        break;
                    */
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
            BulletInfo bullet = bulletPrefabs[idx].GetComponent<BulletInfo>();

            bullet.curShotDelay += Time.deltaTime; // ¿Â¿¸ Ω√∞£

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