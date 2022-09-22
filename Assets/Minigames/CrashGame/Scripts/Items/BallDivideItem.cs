using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class BallDivideItem : Item
    {
        private List<Ball> ballList;
        public override void ActivateItem()
        {
            // 게임 내 모든 공이 두배!
            Debug.Log("BallDivide Item Acquired");
            ballList = new List<Ball>();
            foreach (Transform ball in GameManager.Instance.Item.BallParent)
            {
                ballList.Add(ball.GetComponent<Ball>());
            }
            foreach (Ball ball in ballList)
            {
                Ball newBall = Ball.SpawnBall(ball.rigidBody.position);
                Vector2 velocity = ball.rigidBody.velocity;
                newBall.rigidBody.velocity = Utils.RotateVector(velocity, -15f);
                ball.rigidBody.velocity = Utils.RotateVector(velocity, 15f);
            }
        }
    }
}

