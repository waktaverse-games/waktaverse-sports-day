using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] float speed;
        private void Awake()
        {
            speed = GameSpeedController.instance.speed;
        }

        void Update()
        {
            speed = GameSpeedController.instance.speed;

        }
    }
}
