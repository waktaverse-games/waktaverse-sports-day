using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class BallBrick : ItemDropBrick
    {
        // 파괴 시 공을 생성하는 프리팹.
        private static int scoreAdd = 10;
        private Ball ball;

        private void DropBall()
        {
            if (Ball.BallNumber < 15)
            {
                ball = Ball.SpawnBall((Vector2)transform.position);
                ball.BlockFire();
            }
            //Ball.SpawnTestBall((Vector2)transform.position + centerPosition);
        }

        public override void BallCollide()
        {
            DestroySelf(scoreAdd);
            DropBall();
        }
    }

}
