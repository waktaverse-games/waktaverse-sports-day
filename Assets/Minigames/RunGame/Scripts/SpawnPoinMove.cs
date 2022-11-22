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
            del = 1;
            timer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;

            if (GameHaven.RunGame.GameManager.gameTime < 20)
            {
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
                    del = Random.Range(0, 1.3f);
                }
            }

            else if (GameHaven.RunGame.GameManager.gameTime >= 20 && GameHaven.RunGame.GameManager.gameTime < 40)
            {
                if (timer > del)
                {
                    Speed = Random.Range(4f, 8f);
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
                    del = Random.Range(0, 0.85f);
                }
            }

            else if (GameHaven.RunGame.GameManager.gameTime >= 40)
            {
                if (timer > del)
                {
                    Speed = Random.Range(7f, 10f);
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
                    del = Random.Range(0, 0.3f);
                }
            }


            if (SpawnPoint.transform.position.x < -5)
            {
                SpawnPoint.transform.position = new Vector3(-4.5f, 16.5f, 0.5f);
            }
            else if (SpawnPoint.transform.position.x > 5)
            {
                SpawnPoint.transform.position = new Vector3(4.5f,16.5f,0.5f);
            }


        }

 /*       void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "SpawnWall")
            {
                SpawnPoint.GetComponent<Rigidbody2D>().velocity = -(dir)*Speed;
            }
        }*/
    }
}

