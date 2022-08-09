using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrossGame
{
    public class Player : MonoBehaviour
    {
        Vector3 LandPos;

        BoxCollider2D Collider;

        public bool OnBottom;

        private void Awake()
        {
            Collider = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //전역 설정인 레이어나 태그를 쓰면 안 될 것 같아서 임시
            if (collision.gameObject.GetComponent<Platform>() as Platform)
            {
                OnBottom = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Platform>() as Platform)
            {
                OnBottom = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<Platform>() as Platform)
            {
                OnBottom = false;
            }
        }
    }
}

