using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class PoolManager : MonoBehaviour
    {
        public GameObject[] BulletPrefabs;

        public GameObject[] GuidedBullets;
        int GuidedBulletIdx;
        public GameObject[] SectorBullets;
        int SectorBulletIdx;
        public GameObject[] SlashBullets;
        int SlashBulletIdx;
        public GameObject[] StraightBullets;
        int StraightBulletIdx;

        private void Awake()
        {
            GuidedBullets = new GameObject[20];
            for (int i = 0; i < GuidedBullets.Length; i++) GuidedBullets[i] = Instantiate(BulletPrefabs[0]);

            SectorBullets = new GameObject[20];
            for (int i = 0; i < GuidedBullets.Length; i++) SectorBullets[i] = Instantiate(BulletPrefabs[1]);

            SlashBullets = new GameObject[20];
            for (int i = 0; i < GuidedBullets.Length; i++) SlashBullets[i] = Instantiate(BulletPrefabs[2]);

            StraightBullets = new GameObject[20];
            for (int i = 0; i < GuidedBullets.Length; i++) StraightBullets[i] = Instantiate(BulletPrefabs[3]);
        }

        public GameObject MyInstantiate(int idx, Vector2 pos)
        {
            GameObject[] targetPool = null;

            switch (idx)
            {
                case 0:
                    targetPool = GuidedBullets;
                    break;
                case 1:
                    targetPool = SectorBullets;
                    break;
                case 2:
                    targetPool = SlashBullets;
                    break;
                case 3:
                    targetPool = StraightBullets;
                    break;
            }

            for (int i = 0; i < targetPool.Length; i++)
            {
                if (!targetPool[i].activeSelf)
                {
                    targetPool[i].SetActive(true);
                    targetPool[i].transform.position = pos;
                    return targetPool[i];
                }
            }

            return null;
        }
    }
}
