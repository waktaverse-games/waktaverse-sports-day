using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class ProjectileInfo : MonoBehaviour
    {
        public float speed, attackSpeed;
        public int damage;

        private void Update()
        {
            if (transform.position.x > 10) Destroy(gameObject);
        }
    }
}