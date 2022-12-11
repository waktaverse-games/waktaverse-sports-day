using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.StickyGame
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject[] NPC, coin, vegitable;
        [SerializeField] GameObject cutItem;
        [SerializeField] float spawnCool, vegitableSpawnCool;

        private void Awake()
        {
            Invoke("MyVegitableSpawn", 3.0f);
            StartCoroutine(SpawnVegitableRepeatedly(vegitableSpawnCool));
        }

        void MyVegitableSpawn()
        {
            StartCoroutine(RandomSpawnRepeatedly(spawnCool));
        }

        int idx = 0;
        IEnumerator SpawnVegitableRepeatedly(float sec) // sec > 2�� �̻�
        {
            WaitForSeconds wait = new WaitForSeconds(sec - 2f), wait1dot5sec = new WaitForSeconds(2f);

            while (true)
            {
                yield return wait;

                vegitable[idx].SetActive(true);
                vegitable[idx].transform.position = new Vector2(Random.Range(-3f, 3f), Random.Range(-4f, 4f));

                idx = (idx == 0) ? 1 : 0;

                yield return wait1dot5sec;

                vegitable[idx].SetActive(false);
            }
        }

        IEnumerator RandomSpawnRepeatedly(float sec) // NPC�� �� sec�ʸ��� ������ ��ġ�� ���������� ����
        {
            WaitForSeconds wait = new WaitForSeconds(sec);

            while (true)
            {
                int random = Random.Range(0, 11);
                GameObject obj = NPC[Random.Range(0, NPC.Length)];
                if (random <= 2) obj = coin[Random.Range(0, coin.Length)]; // 20% Ȯ���� ���� ����
                else if (random <= 3) obj = cutItem; // 10% Ȯ���� ���� ������ ����

                Move moveCS = Instantiate(obj, new Vector2(Random.Range(-3f, 3f), Random.Range(-4f, 4f)), Quaternion.Euler(Vector3.zero)).GetComponent<Move>();

                if (obj.Equals(NPC))
                {
                    // NPC ���� �ʱⰪ ����
                    moveCS.isPlayer = false;
                    moveCS.isAssociated = false;
                    moveCS.GetComponent<Collider2D>().isTrigger = true;
                }

                yield return wait;
            }
        }
    }
}
