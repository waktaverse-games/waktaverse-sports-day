using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.SpreadGame
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] float speed;

        [SerializeField] GameObject[] projectiles;
        [SerializeField] int[] projectilesLV;
        [SerializeField] GameObject coinAcquireEffect;

        Rigidbody2D rigid;


        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();

            projectilesLV[0] = projectilesLV[1] = projectilesLV[2] = 0;
            projectilesLV[3] = 1;

            StartCoroutine(Project(projectiles[3])); // 기본 탄환
        }

        private void Update()
        {
            if (rigid.velocity.sqrMagnitude > speed * speed) rigid.velocity = rigid.velocity.normalized * speed;

            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -7, 7), Mathf.Clamp(transform.position.y, -4, 4));
        }
        private void FixedUpdate()
        {
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

            rigid.velocity *= 0.8f;
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
                }
                foreach (GameObject del in GameObject.FindGameObjectsWithTag("UpgradeItem"))
                {
                    Destroy(del);
                }
            }
        }

        IEnumerator Project(GameObject projectile)
        {
            ProjectileInfo projectileInfo = Instantiate(projectile, transform.position, projectile.transform.rotation).GetComponent<ProjectileInfo>();

            if (projectileInfo.type == ProjectileInfo.Type.Sector)
            {
                projectileInfo.rigid.velocity = new Vector2(0.866f, -0.5f) * projectileInfo.speed; // -30도

                yield return new WaitForSeconds(projectileInfo.attackSpeed / 8);
                projectileInfo = Instantiate(projectile, transform.position, transform.rotation).GetComponent<ProjectileInfo>();
                projectileInfo.rigid.velocity = new Vector2(0.9659f, -0.2588f) * projectileInfo.speed; // -15도

                yield return new WaitForSeconds(projectileInfo.attackSpeed / 8);
                projectileInfo = Instantiate(projectile, transform.position, transform.rotation).GetComponent<ProjectileInfo>();
                projectileInfo.rigid.velocity = Vector2.right * projectileInfo.speed; // 0도

                yield return new WaitForSeconds(projectileInfo.attackSpeed / 8);
                projectileInfo = Instantiate(projectile, transform.position, transform.rotation).GetComponent<ProjectileInfo>();
                projectileInfo.rigid.velocity = new Vector2(0.9659f, 0.2588f) * projectileInfo.speed; // 15도

                yield return new WaitForSeconds(projectileInfo.attackSpeed / 8);
                projectileInfo = Instantiate(projectile, transform.position, transform.rotation).GetComponent<ProjectileInfo>();
                projectileInfo.rigid.velocity = new Vector2(0.866f, 0.5f) * projectileInfo.speed; // 30도

                yield return new WaitForSeconds(projectileInfo.attackSpeed / 8);
                projectileInfo = Instantiate(projectile, transform.position, transform.rotation).GetComponent<ProjectileInfo>();
                projectileInfo.rigid.velocity = new Vector2(0.9659f, 0.2588f) * projectileInfo.speed; // 15도

                yield return new WaitForSeconds(projectileInfo.attackSpeed / 8);
                projectileInfo = Instantiate(projectile, transform.position, transform.rotation).GetComponent<ProjectileInfo>();
                projectileInfo.rigid.velocity = Vector2.right * projectileInfo.speed; // 0도

                yield return new WaitForSeconds(projectileInfo.attackSpeed / 8);
                projectileInfo = Instantiate(projectile, transform.position, transform.rotation).GetComponent<ProjectileInfo>();
                projectileInfo.rigid.velocity = new Vector2(0.9659f, -0.2588f) * projectileInfo.speed; // -15도

                yield return new WaitForSeconds(projectileInfo.attackSpeed / 8);
                StartCoroutine(Project(projectile));
            }
            else
            {
                yield return new WaitForSeconds(projectileInfo.attackSpeed);
                if (projectileInfo.type == ProjectileInfo.Type.Guided) yield return new WaitUntil(() => FindObjectOfType<EnemyMove>() != null);
                StartCoroutine(Project(projectile));
            }
        }
    }
}