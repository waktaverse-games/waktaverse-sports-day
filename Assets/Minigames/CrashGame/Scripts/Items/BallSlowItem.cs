using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class BallSlowItem : Item
    {
        // 공의 속도를 모두 초기 속도로 되돌리는 아이템
        public override void ActivateItem()
        {
            Debug.Log("BallSlow Item Acquired!");
            foreach (Transform ball in GameManager.Instance.Item.BallParent)
            {
                ball.GetComponent<Ball>().ResetBallSpeed();
            }
            GameManager.Instance.UI.ShowItemEffect("공의 속도 초기화!");
        }
    }
}
