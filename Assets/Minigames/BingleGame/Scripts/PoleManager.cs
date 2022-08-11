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
                // 게임 매니저에게 점수 전달
                GameManager.instance.IncreaseScore(score);
                // 다른 충돌 오브젝트 비활성화
                transform.parent.GetComponent<CheckPointManager>().DisableOtherCollider();

                // 여기에 다른 캐릭터 생성해서 양옆으로 걸어 나가는 작업 해야 함. 

                // 객체 파괴
                Destroy(gameObject);
            }
        }
    }
}
