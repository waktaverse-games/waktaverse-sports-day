using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField]
        private Transform itemParent;

        public Transform ItemParent
        {
            get { return itemParent; }
        }

        public void DeleteAll()
        {
            foreach (Transform item in itemParent)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
