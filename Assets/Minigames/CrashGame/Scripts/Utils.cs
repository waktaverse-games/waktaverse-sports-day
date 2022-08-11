using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

namespace GameHeaven.CrashGame
{
    public class Utils
    {
        public enum GameState
        {
            Start,
            Over
        }

        public static Vector2 RotateVector(Vector2 vector, float delta)
        {
            float radian = Deg2Rad * delta;
            return new Vector2(vector.x * Cos(radian) - vector.y * Sin(radian), vector.x * Sin(radian) + vector.y * Cos(radian));
        }
    }
}

