using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class ItemDropBrick : Brick
    {
        // 코인 외 다른 아이템을 드랍하는 벽돌.
        [SerializeField]
        protected Item itemPrefab;      // 드랍할 아이템의 프리팹
        protected Sprite itemSprite;    // 벽돌 위에 표기될 아이템 스프라이트

        protected override void Awake()
        {
            base.Awake();
        }


        public override void BallCollide()
        {
            
        }
    }
}
