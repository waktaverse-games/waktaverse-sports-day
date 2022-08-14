using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject testObj;
        [SerializeField] float SpawnCool;
        [SerializeField] float[] mapSize;

        private void Awake()
        {
            StartCoroutine(SpawnRepeatedly(SpawnCool));
        }

        IEnumerator SpawnRepeatedly(float sec)
        {
            Instantiate(testObj, new Vector2(mapSize[0] / 2 + 1, Random.Range(-mapSize[1] / 2, mapSize[1] / 2)), testObj.transform.rotation);

            yield return new WaitForSeconds(sec);
            StartCoroutine(SpawnRepeatedly(sec));
        }
    }
}