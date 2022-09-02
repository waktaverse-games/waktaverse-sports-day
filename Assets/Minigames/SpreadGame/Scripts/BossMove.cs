using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class BossMove : MonoBehaviour
    {
        public enum Type { RaNi }
        public int HP;
        public Type type;

        private Animator anim;
        [SerializeField] int elapsedFrames;
        [SerializeField] GameObject projectile;

        PoolManager pool;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            pool = FindObjectOfType<PoolManager>();

            StartCoroutine(Think(4));
            StartCoroutine(Attack(24, 60000000, 100000000, 5));
        }

        private void Update()
        {

        }
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Attack"))
            {
                anim.SetTrigger("Hit");
                HP -= collider.GetComponent<BulletInfo>().damage;
                if (collider.GetComponent<BulletInfo>().type != BulletInfo.Type.Slash) collider.gameObject.SetActive(false);

                if (HP <= 0) Die();
            }
        }

        IEnumerator Think(float sec)
        {
            WaitForSeconds wait = new WaitForSeconds(sec);

            while (true)
            {
                print(Time.time + " // " + Time.fixedDeltaTime + " // " + Time.deltaTime);
                yield return wait;
                switch (Random.Range(0, 2))
                {
                    case 0: // Pattern0
                        anim.SetTrigger("Pattern0");
                        if (type == Type.RaNi)
                        {
                            yield return new WaitForSeconds(0.1f);
                            StartCoroutine(Attack(1, 12, 0.8f, 7));
                            yield return new WaitForSeconds(1.9f);
                            StartCoroutine(Attack(1, 12, 0.8f, 7));
                            wait = new WaitForSeconds(2.0f);
                        }
                        else
                        {
                            StartCoroutine(Attack(1, 100, sec, 5));
                        }
                        break;
                    case 1: // Pattern 1
                        switch (Random.Range(0, 3))
                        {
                            case 0:
                                anim.SetTrigger("Pattern1_Top");
                                break;
                            case 1:
                                anim.SetTrigger("Pattern1_Middle");
                                break;
                            case 2:
                                anim.SetTrigger("Pattern1_Bottom");
                                break;
                        }
                        wait = new WaitForSeconds(4.0f);
                        break;
                }
            }
        }

        IEnumerator Attack(int num, int times, float sec, float speed) // sec초 안에, 360도 방향으로, num 개수를 times 번 
        {
            WaitForSeconds wait = new WaitForSeconds(sec / times);

            for (int i = 0; i < times; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    pool.MyInstantiate(4, transform.position).GetComponent<Rigidbody2D>().
                        velocity = Quaternion.AngleAxis(j * (360 / num), Vector3.forward) * new Vector2(-speed, 0);
                }
                yield return wait;
            }
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}
