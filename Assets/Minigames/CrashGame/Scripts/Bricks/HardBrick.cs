using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class HardBrick : CoinDropBrick
    {
        // 3번 치면 부서지는 벽돌
        private static int scoreAdd = 20;
        private int brickHealth = 3;
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private List<Sprite> hardBrickSprites;
        private int spriteIndex;

        protected override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteIndex = 0;
            SetHardBrickSprite(spriteIndex);
        }

        public override void BallCollide()
        {
            if (--brickHealth <= 0)
            {
                DestroySelf(scoreAdd);
            }
            else if (spriteIndex < 2) SetHardBrickSprite(++spriteIndex);
            Debug.Log($"brick health: {brickHealth}");
        }

        private void SetHardBrickSprite(int index)
        {
            spriteRenderer.sprite = hardBrickSprites[index];
        }
    }
}
