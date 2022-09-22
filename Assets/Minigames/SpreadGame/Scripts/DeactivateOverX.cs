using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class DeactivateOverX : MonoBehaviour
    {
        [SerializeField] private int x;

        private void Update()
        {
            if (transform.position.x > x || transform.position.x < x) gameObject.SetActive(false);
        }
    }
}
