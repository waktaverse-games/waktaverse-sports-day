using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class ItemBrick : ItemDropBrick
    {
        private static int scoreAdd = 5;
        private ItemManager itemManager;
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

        public Item ItemPrefab
        {
            get { return itemPrefab; }
            set 
            {
                itemPrefab = value;
                itemSprite = itemPrefab.GetComponent<SpriteRenderer>().sprite;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = itemSprite;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            itemManager = GameManager.Instance.Item;
        }

        private void Start()
        {
            BrickColor = GameManager.Instance.Brick.brickColorArray[Random.Range(0, 6)];
        }

        private void DropItem()
        {
            Item itemObject = Instantiate(itemPrefab, (Vector2)transform.position, Quaternion.identity);
            itemObject.transform.SetParent(itemManager.ItemParent, true);
        }

        public override void BallCollide()
        {
            DropItem();
            DestroySelf(scoreAdd);
        }
    }
}
