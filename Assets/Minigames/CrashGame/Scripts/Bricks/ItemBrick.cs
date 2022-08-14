using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class ItemBrick : Brick
    {
        private static int scoreAdd = 5;

        public Item itemPrefab;

        private void DropItem()
        {
            Instantiate(itemPrefab);
        }

        public override void BallCollide()
        {
            DropItem();
            DestroySelf(scoreAdd);
        }
    }
}
