using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class ArrowManager : MonoBehaviour
    {
        [SerializeField] GameObject normalArrow;
        [SerializeField] GameObject reverseArrow;

        private void Start()
        {
            GameObject arrow = Instantiate(normalArrow, transform.position, transform.rotation);
            Destroy(arrow, 3f);
        }
        public void ChangeArrow(bool isReverse)
        {
            GameObject arrow;
            if(isReverse)
            {
                arrow = Instantiate(reverseArrow, transform.position, transform.rotation);
            }
            else
            {
                arrow = Instantiate(normalArrow, transform.position, transform.rotation);
            }
            Destroy(arrow, 1f);
        }
    }
}
