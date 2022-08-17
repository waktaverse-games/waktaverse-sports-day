using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.StickyGame
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject[] NPC, coin;
        [SerializeField] GameObject cutItem;
        [SerializeField] float spawnCool;

        private void Awake()
        {
            StartCoroutine(RandomSpawnRepeatedly(spawnCool));
        }

        IEnumerator RandomSpawnRepeatedly(float sec) // NPC�� �� sec�ʸ��� ������ ��ġ�� ���������� ����
        {
            while (true)
            {
                int random = Random.Range(0, 11);
                GameObject obj = NPC[Random.Range(0, NPC.Length)];
                if (random <= 2) obj = coin[Random.Range(0, coin.Length)]; // 20% Ȯ���� ���� ����
                else if (random <= 3) obj = cutItem; // 10% Ȯ���� ���� ������ ����

                Move moveCS = Instantiate(obj, new Vector2(Random.Range(-3, 4), Random.Range(-4, 5)), Quaternion.Euler(Vector3.zero)).GetComponent<Move>();

                if (obj.Equals(NPC))
                {
                    // NPC ���� �ʱⰪ ����
                    moveCS.isPlayer = false;
                    moveCS.isAssociated = false;
                    moveCS.GetComponent<Collider2D>().isTrigger = true;
                }

                yield return new WaitForSeconds(sec);
            }
        }
    }
}
