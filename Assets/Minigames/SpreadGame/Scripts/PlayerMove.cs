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

        Rigidbody2D rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();

            foreach (GameObject projectile in projectiles)
            {
                StartCoroutine(Project(projectile));
            }
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