﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class ItemBrick : ItemDropBrick
    {
        private static int scoreAdd = 5;

        private Color brickColor;

        public Color BrickColor
        {
            get { return brickColor; }
            set
            {
                brickColor = value;
                GetComponent<SpriteRenderer>().color = value;
            }
        }

        private void Start()
        {
            BrickColor = GameManager.Instance.Brick.brickColorArray[Random.Range(0, 6)];
        }

        private void DropItem()
        {
            Instantiate(itemPrefab, (Vector2)transform.position + centerPosition, Quaternion.identity);
        }

        public override void BallCollide()
        {
            DropItem();
            DestroySelf(scoreAdd);
        }
    }
}
