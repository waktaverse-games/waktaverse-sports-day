using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class StraightMove : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.left;
        }
    }
}
