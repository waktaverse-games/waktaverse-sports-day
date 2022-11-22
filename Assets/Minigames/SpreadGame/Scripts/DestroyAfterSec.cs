using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class DestroyAfterSec : MonoBehaviour
    {
        [SerializeField] int sec;

        private void Awake()
        {
            Invoke("MyDestroy", sec);
        }

        void MyDestroy()
        {
            Destroy(gameObject);
        }
    }
}
