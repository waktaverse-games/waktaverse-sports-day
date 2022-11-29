using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{
    public class SpawnPoinMove : MonoBehaviour
    {
        public GameObject SpawnPoint;
        public GameManager gameManager;

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
            if (gameManager.gameStop == false && gameManager.gameStart == true)
            {
                timer += Time.deltaTime;

                if (gameManager.gameTime < 20)
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

                else if (gameManager.gameTime >= 20 && gameManager.gameTime < 40)
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

                else if (gameManager.gameTime >= 40 && gameManager.gameTime < 80)
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
                else
                {
                    if (timer > del)
                    {
                        Speed = Random.Range(3f, 12f);
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
                        del = Random.Range(0, 0.6f);
                    }
                }



                if (SpawnPoint.transform.position.x < -7)
                {
                    SpawnPoint.transform.position = new Vector3(-6.5f, 16.5f, 0.5f);
                }
                else if (SpawnPoint.transform.position.x > 7)
                {
                    SpawnPoint.transform.position = new Vector3(6.5f, 16.5f, 0.5f);
                }

            }


        }

    }
}

