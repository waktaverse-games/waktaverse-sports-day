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
                transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = itemSprite;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            itemManager = MiniGameManager.Instance.Item;
        }

        private void Start()
        {
            BrickColor = MiniGameManager.Instance.Brick.brickColorArray[Random.Range(0, 6)];
        }

        private void DropItem()
        {
            Item itemObject = MiniGameManager.ObjectPool.GetObject(ItemPrefab.GetName()).GetComponent<Item>();
            itemObject.transform.position = transform.position;
            itemObject.transform.SetParent(itemManager.ItemParent, true);
        }

        protected override void DestroySelf(int score)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            base.DestroySelf(score);
        }

        public override void BallCollide()
        {
            DropItem();
            DestroySelf(scoreAdd);
        }
    }
}
