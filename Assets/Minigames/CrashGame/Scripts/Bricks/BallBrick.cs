using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class BallBrick : Brick
    {
        private static int scoreAdd = 10;
        private Ball ball;

        private void DropBall()
        {
            ball = Ball.SpawnBall((Vector2)transform.position + centerPosition);
            ball.BlockFire();
            //Ball.SpawnTestBall((Vector2)transform.position + centerPosition);
        }

        public override void BallCollide()
        {
            DestroySelf(scoreAdd);
            DropBall();
        }
    }

}
