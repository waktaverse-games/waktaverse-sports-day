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
                // 게임 매니저에게 점수 전달
                GameManager.instance.IncreaseScore(score);
                // 다른 충돌 오브젝트 비활성화
                transform.parent.GetComponent<CheckPointManager>().DisableOtherCollider();

            }
        }
    }
}