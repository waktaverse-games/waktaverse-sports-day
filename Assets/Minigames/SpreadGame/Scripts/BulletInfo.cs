using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameHeaven.SpreadGame
{
    public class BulletInfo : MonoBehaviour
    {
        public enum Type { Straight, Guided, Sector, Slash }

        public Type type;
        public float speed, curShotDelay, maxShotDelay;
        public int damage;
    }
}