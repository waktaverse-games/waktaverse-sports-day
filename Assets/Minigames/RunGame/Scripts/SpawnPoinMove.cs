using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{
    public class SpawnPoinMove : MonoBehaviour
    {
        public GameObject SpawnPoint;

        float Speed;
        float timer;
        float del;

        Vector2 dir = Vector2.left;
        bool Dirleft = true;

        void Start()
        {
            Speed = 0;
            del = 3;
            timer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;

            if (timer > del)
            {
                Speed = Random.Range(2f, 4.5f);
                if (Dirleft == true)
                {
                    Dirleft = false;
                    dir = Vector2.right;
                }
                else
                {
                    Dirleft = true;
                    dir = Vector2.left;
                }

                SpawnPoint.GetComponent<Rigidbody2D>().velocity = dir * Speed;
                timer = 0;
                del = Random.Range(0, 1f);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "SpawnWall")
            {
                SpawnPoint.GetComponent<Rigidbody2D>().velocity = -(dir)*Speed;
            }
        }
    }
}

