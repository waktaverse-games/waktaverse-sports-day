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

        private static Color[] colorArray = {Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan};

        // Start is called before the first frame update

        private void Start()
        {
            BrickColor = colorArray[Random.Range(0, 6)];
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
