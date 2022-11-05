using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.JumpGame
{
    public class ItemPoolConnector : MonoBehaviour
    {
        public ItemSpawner Spawner;
        public void DisableItem()
        {
            Spawner.DeactiavteItem(gameObject);
        }
    }
}
