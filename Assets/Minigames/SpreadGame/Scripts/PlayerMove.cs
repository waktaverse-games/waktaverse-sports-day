using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameHeaven.SpreadGame
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] float speed;

        [SerializeField] GameObject projectile;

        Rigidbody2D rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();

            StartCoroutine(Project(projectile));
        }

        private void Update()
        {
            if (rigid.velocity.sqrMagnitude > speed * speed) rigid.velocity = rigid.velocity.normalized * speed;

            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -7.5f, 7.5f), Mathf.Clamp(transform.position.y, -4.5f, 4.5f));
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
            ProjectileInfo projectileInfo = Instantiate(projectile, transform.position, transform.rotation).GetComponent<ProjectileInfo>();

            projectileInfo.GetComponent<Rigidbody2D>().velocity = Vector2.right * projectileInfo.speed;

            yield return new WaitForSeconds(projectileInfo.attackSpeed);
            StartCoroutine(Project(projectile));
        }
    }
}