using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class PassAreaManager : MonoBehaviour
    {
        public int score;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                // ���� �Ŵ������� ���� ����
                GameManager.instance.IncreaseScore(score);
                // �ٸ� �浹 ������Ʈ ��Ȱ��ȭ
                transform.parent.GetComponent<CheckPointManager>().DisableOtherCollider();

            }
        }
    }
}