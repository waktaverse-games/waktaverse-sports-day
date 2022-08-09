using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject NPC, coin, cutItem;

    private void Awake()
    {
        StartCoroutine(RandomSpawnRepeatedly(4.0f));
    }

    IEnumerator RandomSpawnRepeatedly(float sec) // NPC�� �� sec�ʸ��� ������ ��ġ�� ���������� ����
    {
        int random = Random.Range(0, 11);
        GameObject obj = NPC;
        if (random <= 2) obj = coin; // 20% Ȯ���� ���� ����
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
        StartCoroutine(RandomSpawnRepeatedly(sec));
    }
}
