using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class RunningCollider : MonoBehaviour
    {
        private void OnEnable()
        {
            GameObject[] otherObjects = GameObject.FindGameObjectsWithTag("Other");
            foreach (GameObject obj in otherObjects)
            {
                Physics2D.IgnoreCollision(obj.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            }
        }
        
    }
}

