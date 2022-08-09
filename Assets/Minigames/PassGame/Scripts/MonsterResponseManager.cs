using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.PassGame
{
    public class MonsterResponseManager : MonoBehaviour
    {
        public GameObject MonsterPrefab = null;
        public int MonsterCount = 5;

        private List<GameObject> _monsterPool = new List<GameObject>();
        private int _createCount = 0;
        private void Awake()
        {
            if (MonsterPrefab)
            {
                for (int i = 0; i < MonsterCount; i++)
                {
                    _monsterPool.Add(InitMonster(MonsterPrefab, this.transform));
                }
            }
        }

        private void Start()
        {
            StartCoroutine(CreateMonster());
        }
        
        // Coroutine
        IEnumerator CreateMonster()
        {
            while (true)
            {
                _monsterPool[_createCount % _monsterPool.Count].SetActive(true);
                _createCount++;
                yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));
            }
        }

        GameObject InitMonster(GameObject obj, Transform parent)
        {
            GameObject copy = Instantiate(obj);
            copy.transform.SetParent(parent);
            copy.SetActive(false);
            return copy;
        }
    }
}
