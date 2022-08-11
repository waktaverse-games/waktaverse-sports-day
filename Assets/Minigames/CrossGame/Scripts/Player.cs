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
            //���� ������ ���̾ �±׸� ���� �� �� �� ���Ƽ� �ӽ�
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
