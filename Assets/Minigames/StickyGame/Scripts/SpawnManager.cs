using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.StickyGame
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject NPC, coin, cutItem;

        private void Awake()
        {
            StartCoroutine(RandomSpawnRepeatedly(4.0f));
        }

        IEnumerator RandomSpawnRepeatedly(float sec) // NPC를 매 sec초마다 랜덤한 위치에 랜덤아이템 생성
        {
            int random = Random.Range(0, 11);
            GameObject obj = NPC;
            if (random <= 2) obj = coin; // 20% 확률로 코인 생성
            else if (random <= 3) obj = cutItem; // 10% 확률로 제거 아이템 생성

            Move moveCS = Instantiate(obj, new Vector2(Random.Range(-3, 4), Random.Range(-4, 5)), Quaternion.Euler(Vector3.zero)).GetComponent<Move>();

            if (obj.Equals(NPC))
            {
                // NPC 변수 초기값 세팅
                moveCS.isPlayer = false;
                moveCS.isAssociated = false;
                moveCS.GetComponent<Collider2D>().isTrigger = true;
            }

            yield return new WaitForSeconds(sec);
            StartCoroutine(RandomSpawnRepeatedly(sec));
        }
    }
}
