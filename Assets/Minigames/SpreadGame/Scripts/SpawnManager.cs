using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] GameObject testObj;
        [SerializeField] float SpawnCool;

        private void Awake()
        {
            StartCoroutine(SpawnRepeatedly(SpawnCool));
        }

        IEnumerator SpawnRepeatedly(float sec)
        {
            Instantiate(testObj, new Vector2(9, Random.Range(-4, 5)), testObj.transform.rotation);

            yield return new WaitForSeconds(sec);
            StartCoroutine(SpawnRepeatedly(sec));
        }
    }
}