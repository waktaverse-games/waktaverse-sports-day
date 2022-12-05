using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{
    public class WallControl : MonoBehaviour
    {
        Rigidbody2D rig;

        GameManager gameManager;
        public GameObject main;
        GameObject tree1;
        GameObject tree2;

        Animator move1;
        Animator move2;

            void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            rig = GetComponent<Rigidbody2D>();
            rig.velocity = Vector2.down * gameManager.wallSpeed;

            if (main.name.Contains("Spawn"))
            {
                tree1 = main.transform.Find("Tree").gameObject;
                tree2 = main.transform.Find("Tree (1)").gameObject;

                move1 = tree1.GetComponent<Animator>();
                move2 = tree2.GetComponent<Animator>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (gameManager.gameStop == false && gameManager.gameStart == true)
            {
                if (main.name.Contains("Spawn"))
                {
                    move1.SetBool("Stop", false);
                    move2.SetBool("Stop", false);
                }

                switch (gameManager.gameTime)
                {
                    case < 10:
                        gameManager.wallSpeed += 0.02f * Time.deltaTime;
                        break;
                    case >= 10 when gameManager.wallSpeed < 20:
                        gameManager.wallSpeed += 0.008f * Time.deltaTime;
                        break;
                }
            }
            else
            {
                rig.velocity = Vector2.down * 0;
                if (main.name.Contains("Spawn"))
                {
                    move1.SetBool("Stop", true);
                    move2.SetBool("Stop", true);
                }
            }
        }

        private void FixedUpdate()
        {
            rig.velocity = Vector2.Lerp(rig.velocity, Vector2.down * gameManager.wallSpeed, 0.5f);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Outline")
                Destroy(gameObject);
        }
    }

}