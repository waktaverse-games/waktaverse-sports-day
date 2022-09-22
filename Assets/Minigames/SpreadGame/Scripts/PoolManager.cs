using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class PoolManager : MonoBehaviour
    {
        public GameObject[] bulletPrefabs;
        private int[] bulletBuffers;

        public GameObject[] guidedBullets; // 0
        public GameObject[] sectorBullets; // 1
        public GameObject[] slashBullets; // 2
        public GameObject[] straightBullets; // 3
        public GameObject[] bananaBullets; // 4
        public GameObject[] poopBullets; // 5
        public GameObject[] cageBullets; // 6

        private void Awake()
        {
            bulletBuffers = new int[bulletPrefabs.Length];

            guidedBullets = new GameObject[20];
            for (int i = 0; i < guidedBullets.Length; i++) guidedBullets[i] = Instantiate(bulletPrefabs[0]);

            sectorBullets = new GameObject[100];
            for (int i = 0; i < sectorBullets.Length; i++) sectorBullets[i] = Instantiate(bulletPrefabs[1]);

            slashBullets = new GameObject[20];
            for (int i = 0; i < slashBullets.Length; i++) slashBullets[i] = Instantiate(bulletPrefabs[2]);

            straightBullets = new GameObject[100];
            for (int i = 0; i < straightBullets.Length; i++) straightBullets[i] = Instantiate(bulletPrefabs[3]);

            bananaBullets = new GameObject[500];
            for (int i = 0; i < bananaBullets.Length; i++) bananaBullets[i] = Instantiate(bulletPrefabs[4]);

            poopBullets = new GameObject[20];
            for (int i = 0; i < poopBullets.Length; i++) poopBullets[i] = Instantiate(bulletPrefabs[5]);

            cageBullets = new GameObject[3];
            for (int i = 0; i < cageBullets.Length; i++) cageBullets[i] = Instantiate(bulletPrefabs[6]);
        }

        public GameObject MyInstantiate(int idx, Vector2 pos)
        {
            GameObject[] targetPool = null;

            switch (idx)
            {
                case 0:
                    targetPool = guidedBullets;
                    break;
                case 1:
                    targetPool = sectorBullets;
                    break;
                case 2:
                    targetPool = slashBullets;
                    break;
                case 3:
                    targetPool = straightBullets;
                    break;
                case 4:
                    targetPool = bananaBullets;
                    break;
                case 5:
                    targetPool = poopBullets;
                    break;
                case 6:
                    targetPool = cageBullets;
                    break;
            }

            if (bulletBuffers[idx] >= targetPool.Length) bulletBuffers[idx] = 0;

            for (; bulletBuffers[idx] < targetPool.Length; bulletBuffers[idx]++)
            {
                if (!targetPool[bulletBuffers[idx]].activeSelf)
                {
                    targetPool[bulletBuffers[idx]].SetActive(true);
                    targetPool[bulletBuffers[idx]].transform.position = pos;
                    return targetPool[bulletBuffers[idx]++];
                }
            }

            return null;
        }
    }
}
