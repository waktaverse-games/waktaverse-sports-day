using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.CrashGame
{
    public class BasicBrick : CoinDropBrick
    {
        private static int scoreAdd = 5;
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

        // 이세돌 퍼스널 컬러
        //private static Color[] colorArray = {new Color(138, 43, 226), new Color(240, 169, 87), new Color(0, 0, 128), new Color(128, 0, 128), new Color(70, 126, 198), new Color(133, 172, 32)};

        // Start is called before the first frame update

        private void Start()
        {
            BrickColor = GameManager.Instance.Brick.brickColorArray[Random.Range(0, 6)];
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void BallCollide()
        {
            DestroySelf(scoreAdd);
        }
    }

}
