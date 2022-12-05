using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class ClaymoreItem : Item
    {
        private BrickManager brickManager;
        [SerializeField]
        private SmallBullet bulletPrefab;

        protected override void Awake()
        {
            base.Awake();
            brickManager = MiniGameManager.Instance.Brick;
        }

        //private void BreakBrick(int number)
        //{
        //    List<Brick> outerLine = brickManager.BrickMap.Peek();

        //    int brokeNumber = 0;
        //    for (int i = 0; i < number; i++)
        //    {

        //    }
        //}

        private void SpawnBullet()
        {
            Vector2 spawnPosition = (Vector2)MiniGameManager.Instance.platform.transform.GetChild(0).position;
            Vector2 bulletFireDirection = new Vector2(0f, 1f);
            bulletFireDirection = Utils.RotateVector(bulletFireDirection, -20);
            for (int i = 0; i < 5; i++) 
            {
                SmallBullet newBullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
                newBullet.transform.SetParent(MiniGameManager.Instance.Item.ItemParent);
                newBullet.FireBullet(bulletFireDirection);
                bulletFireDirection = Utils.RotateVector(bulletFireDirection, 10);
            }
        }

        public override void ActivateItem()
        {
            Debug.Log("Claymore Acquired!");
            SpawnBullet();
            MiniGameManager.Instance.UI.ShowItemEffect("수류탄 폭발!");
        }
    }
}
