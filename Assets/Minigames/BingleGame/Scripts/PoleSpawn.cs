using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleSpawn : MonoBehaviour
{
    public GameObject[] leftSpawnPoints;
    public GameObject[] rightSpawnPoints;
    public GameObject pole;

    public float maxSpawnDelay;

    public float maxGap;
    public float minGap;

    [SerializeField] float curTime;
    [SerializeField] bool isLeft;
    
    private void Update()
    {
        curTime += Time.deltaTime;

        if(curTime > maxSpawnDelay)
        {
            SpawnPoles();
        }
    }

    private void SpawnPoles()
    {
        if (isLeft) // 좌측에 막대 생성
        {
            isLeft = false;
            float leftSpawnXCoord = Random.Range(leftSpawnPoints[0].transform.position.x, leftSpawnPoints[1].transform.position.x);
            float rightSpwanXCoord = leftSpawnXCoord + Random.Range(minGap, maxGap);
            Instantiate(pole, new Vector3(leftSpawnXCoord, transform.position.y, 0), transform.rotation);
            Instantiate(pole, new Vector3(rightSpwanXCoord, transform.position.y, 0), transform.rotation);
        }
        else //우측에 막대 생성
        {
            isLeft = true;
            float rightSpawnXCoord = Random.Range(rightSpawnPoints[0].transform.position.x, rightSpawnPoints[1].transform.position.x);
            float leftSpawnXCoord = rightSpawnXCoord - Random.Range(minGap, maxGap);
            Instantiate(pole, new Vector3(leftSpawnXCoord, transform.position.y, 0), transform.rotation);
            Instantiate(pole, new Vector3(rightSpawnXCoord, transform.position.y, 0), transform.rotation);
        }
        curTime = 0;
    }
}
