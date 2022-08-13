using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{
    public class WallControl : MonoBehaviour
    {
        public float speed;

        Rigidbody2D rig;

        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            rig.velocity = Vector2.down * speed;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Outline")
                Destroy(gameObject);
        }
    }

}