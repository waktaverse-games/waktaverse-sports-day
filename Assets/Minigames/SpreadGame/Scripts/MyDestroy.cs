using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class MyDestroy : MonoBehaviour
    {
        [SerializeField] private float cool;

        private void Awake()
        {
            if (cool > 0) Invoke("MyDestroyFunc", cool);
        }

        private void Update()
        {
            if (transform.position.x < -7 || transform.position.x > 7
                || transform.position.y <-4 || transform.position.y > 4) Destroy(gameObject);
        }

        void MyDestroyFunc()
        {
            Destroy(gameObject);
        }
    }
}
