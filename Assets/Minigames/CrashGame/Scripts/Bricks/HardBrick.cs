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
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteIndex = 0;
            SetHardBrickSprite(spriteIndex);
        }

        public override void BallCollide()
        {
            brickHealth--;
            // 임시로 색깔만 어두워지게 수정
            //spriteRenderer.color = new Color(spriteRenderer.color.r * 0.8f, spriteRenderer.color.g * 0.8f, spriteRenderer.color.b * 0.8f, spriteRenderer.color.a);
            if (spriteIndex < 2) SetHardBrickSprite(++spriteIndex);
            if (brickHealth <= 0)
            {
                DestroySelf(scoreAdd);
            }
        }

        private void SetHardBrickSprite(int index)
        {
            spriteRenderer.sprite = hardBrickSprites[index];
        }
    }
}
