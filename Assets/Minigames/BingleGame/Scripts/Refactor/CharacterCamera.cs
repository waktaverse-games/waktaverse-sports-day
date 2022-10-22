using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField] Transform player;

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(0, player.position.y - 2.5f, -10);
        }
    }
}
