using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class ItemMove : MonoBehaviour
    {
        Rigidbody2D rigid;
        public Vector3 dir;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();

            dir = new Vector3(0, 0.05f, 0);

            StartCoroutine(ChangeDir());
        }

        private void Update()
        {
            rigid.AddForce(dir, ForceMode2D.Impulse);

            rigid.velocity = new Vector2(-1, rigid.velocity.y);

            if (rigid.velocity.y < -1) rigid.velocity = new Vector2(rigid.velocity.x, -1);
            else if (rigid.velocity.y > 1) rigid.velocity = new Vector2(rigid.velocity.x, 1);
        }

        IEnumerator ChangeDir()
        {
            WaitForSeconds wait = new WaitForSeconds(1.0f);

            while (true)
            {
                yield return wait;
                dir = new Vector3(0, -dir.y, 0);
            }
        }
    }
}
