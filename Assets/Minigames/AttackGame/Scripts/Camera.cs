using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.AttackGame
{
    public class Camera : MonoBehaviour
    {
        public GameObject player;

        public bool isStageChanging = false;

        public bool isGamePlaying = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (isGamePlaying)
            {
                float playerX = player.transform.position.x;
                Vector3 currentPos = transform.position;
                if ((playerX > currentPos.x && currentPos.x < 38.4f && !isStageChanging) ||
                    (playerX > currentPos.x && isStageChanging))
                {
                    Vector3 newVec = new Vector3(playerX, currentPos.y, currentPos.z);
                    transform.position = newVec;
                }
            }
        }
    }
}

