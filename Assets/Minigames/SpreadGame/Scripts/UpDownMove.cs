using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class UpDownMove : MonoBehaviour
    {
        Rigidbody2D rigid;
        public Vector3 dir;
        public bool isBoss;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();

            dir = new Vector3(0, 0.02f, 0);

            StartCoroutine(ChangeDir());
        }

        public IEnumerator BossItemMove(float sec)
        {
            yield return new WaitForSeconds(sec);

            rigid.velocity = Vector2.left * 5;
        }

        private void Update()
        {
            if (isBoss) return;

            rigid.AddForce(dir, ForceMode2D.Impulse);

            rigid.velocity = new Vector2(-1, rigid.velocity.y);

            if (rigid.velocity.y < -1) rigid.velocity = new Vector2(rigid.velocity.x, -1);
            else if (rigid.velocity.y > 1) rigid.velocity = new Vector2(rigid.velocity.x, 1);
        }

        IEnumerator ChangeDir()
        {
            WaitForSeconds wait = new WaitForSeconds(1.0f);

            yield return new WaitForSeconds(0.5f);
            while (true)
            {
                dir = new Vector3(0, -dir.y, 0);
                yield return wait;
            }
        }
    }
}
