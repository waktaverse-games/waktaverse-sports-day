using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject runner;

    private void Awake()
    {
        StartCoroutine(RandomSpawnRepeatedly(4.0f));
    }

    IEnumerator RandomSpawnRepeatedly(float sec) // NPC�� �� sec�ʸ��� ��������
    {
        Move moveCS = Instantiate(runner, new Vector2(Random.Range(-4, 5), Random.Range(-4, 5)), Quaternion.Euler(Vector3.zero)).GetComponent<Move>();

        // Instantiate�� NPC ���� �ʱⰪ ����
        moveCS.isPlayer = false;
        moveCS.isAssociated = false;
        moveCS.GetComponent<Collider2D>().isTrigger = true;

        yield return new WaitForSeconds(sec);
        StartCoroutine(RandomSpawnRepeatedly(sec));
    }
}
