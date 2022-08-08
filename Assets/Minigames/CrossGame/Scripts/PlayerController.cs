using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameHeaven.CrossGame {
    public class PlayerController : MonoBehaviour
    {
        Vector3 LandPos;
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        public void Jump()
        {
            //transform.DOJump(Vector3.right, 3f, 1, 1).SetRelative();
            //transform.DOMoveX(3, 1);
            //transform.DOMoveX(-3, 1);
        }
    }
}

