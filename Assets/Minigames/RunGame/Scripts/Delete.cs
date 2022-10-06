using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHaven.RunGame
{
    public class Delete : MonoBehaviour
    {
        Rigidbody2D rig;

        void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            rig.velocity = Vector2.down * GameHaven.RunGame.GameManager.wallSpeed/4;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name == "Outline")
                Destroy(gameObject);
        }
    }
}
