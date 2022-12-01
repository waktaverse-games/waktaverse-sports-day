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
            Countdown,
            Start,
            Over
        }

        /// <summary>
        /// 360도 기준 두 벡터 간의 각도를 반환
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        /// <returns>360도 기준 두 벡터 간의 각도</returns>
        public static float Angle360(Vector2 from, Vector2 to)
        {
            float angle = Vector2.SignedAngle(from, to);
            if (angle < 0) angle += 360;
            return angle;
        }

        /// <summary>
        /// Vector2 벡터 vector를 delta(degree)만큼 회전한 벡터를 반환
        /// </summary>
        /// <param name="vector">회전시킬 벡터</param>
        /// <param name="delta">회전할 각도(degree)</param>
        /// <returns>delta(degree)만큼 회전된 vector</returns>
        public static Vector2 RotateVector(Vector2 vector, float delta)
        {
            float radian = Deg2Rad * delta;
            return new Vector2(vector.x * Cos(radian) - vector.y * Sin(radian), vector.x * Sin(radian) + vector.y * Cos(radian));
        }

        /// <summary>
        /// Vector2 벡터 vector를 delta(degree)만큼 회전한 벡터를 반환
        /// 단, 절대각도(x축 양의 방향 기준) clamp보다 많이 회전할 경우 clamp까지만 회전한 벡터를 반환
        /// </summary>
        /// <param name="vector">회전할 벡터</param>
        /// <param name="delta">회전할 각도(degree)</param>
        /// <param name="clamp">회전 한계 각도(x축 양의 방향 기준)</param>
        /// <returns>delta만큼 회전한 벡터 (최대 clamp)</returns>
        public static Vector2 RotateClampVector(Vector2 vector, float delta, float clamp)
        {
            float radian = Deg2Rad * delta;
            Vector2 returnVector = new Vector2(vector.x * Cos(radian) - vector.y * Sin(radian), vector.x * Sin(radian) + vector.y * Cos(radian));
            if (delta > 0)
            {
                if (Angle360(returnVector, Vector2.right) > clamp)
                {
                    return RotateVector(Vector2.right * vector.magnitude, clamp);
                }
                else return returnVector;
            }
            else
            {
                if (Angle360(returnVector, Vector2.right) < clamp)
                {
                    return RotateVector(Vector2.right * vector.magnitude, clamp);
                }
                else return returnVector;
            }
        }
    }
}

