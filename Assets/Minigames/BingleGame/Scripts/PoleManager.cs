using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.BingleGame
{
    public class PoleManager : MonoBehaviour
    {
        public int score;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Collision!");
                // ���� �Ŵ������� ���� ����
                GameManager.instance.IncreaseScore(score);
                // �ٸ� �浹 ������Ʈ ��Ȱ��ȭ
                transform.parent.GetComponent<CheckPointManager>().DisableOtherCollider();

                // ���⿡ �ٸ� ĳ���� �����ؼ� �翷���� �ɾ� ������ �۾� �ؾ� ��. 

                // ��ü �ı�
                Destroy(gameObject);
            }
        }
    }
}
