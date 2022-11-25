using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{
    public class WallControl : MonoBehaviour
    {
        Rigidbody2D rig;

        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            rig.velocity = Vector2.down * GameHaven.RunGame.GameManager.wallSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            switch (GameManager.gameTime)
            {
                case < 10:
                    GameManager.wallSpeed += 0.06f * Time.deltaTime;
                    break;
                case >= 10 when GameManager.wallSpeed < 30:
                    GameManager.wallSpeed += 0.015f * Time.deltaTime;
                    break;
            }
            
            // if (GameHaven.RunGame.GameManager.gameTime < 10)
            // {
            //     GameHaven.RunGame.GameManager.wallSpeed += 0.06f * Time.deltaTime;
            //     rig.velocity = Vector2.down * GameHaven.RunGame.GameManager.wallSpeed;
            // }
            // else if(GameHaven.RunGame.GameManager.gameTime >= 10 && GameHaven.RunGame.GameManager.wallSpeed <30)
            // {
            //     GameHaven.RunGame.GameManager.wallSpeed += 0.015f * Time.deltaTime;
            //     rig.velocity = Vector2.down * GameHaven.RunGame.GameManager.wallSpeed;
            // }
            // else
            //     rig.velocity = Vector2.down * GameHaven.RunGame.GameManager.wallSpeed;
        }

        private void FixedUpdate()
        {
            rig.velocity = Vector2.Lerp(rig.velocity, Vector2.down * GameManager.wallSpeed, 0.5f);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Outline")
                Destroy(gameObject);
        }
    }

}