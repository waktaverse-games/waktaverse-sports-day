using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    public GameObject[] prefabs;
    public List<GameObject> objectPool;

    public float xoffsetValue;
    public float yoffsetValue;
    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            var item = Instantiate(prefabs[i]);
            item.transform.SetParent(spawnPoint);
            item.transform.localPosition = spawnPoint.localPosition;
            item.SetActive(false);
            objectPool.Add(item);
        }
    }
    public void SpawnObject()
    {
        ResetObject();
        float xOffset = Random.Range(-xoffsetValue, xoffsetValue);
        float yOffset = yoffsetValue >= 0 ? Random.Range(0, yoffsetValue) : Random.Range(yoffsetValue, 0);
        int obstacleIndex = Random.Range(0, prefabs.Length);
        objectPool[obstacleIndex].transform.localPosition = new Vector3(xOffset, yOffset, spawnPoint.localPosition.z);
        objectPool[obstacleIndex].SetActive(true);
    }

    public void ResetObject()
    {
        foreach(var obj in objectPool)
        {
            obj.GetComponent<SpriteRenderer>().enabled = true;
            obj.SetActive(false);
        }
    }
}
