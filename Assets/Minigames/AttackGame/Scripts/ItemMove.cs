using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class ItemMove : MonoBehaviour
    {
        public Transform playerTrans;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (playerTrans.position.x > transform.position.x)
            {
                transform.Translate(4 * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.Translate(-4 * Time.deltaTime, 0, 0);
            }
        }
    }
}