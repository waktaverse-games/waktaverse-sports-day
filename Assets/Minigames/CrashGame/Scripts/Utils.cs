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
        /// 360�� ���� �� ���� ���� ������ ��ȯ
        /// </summary>
        /// <param name="from">The vector from which the angular difference is measured.</param>
        /// <param name="to">The vector to which the angular difference is measured.</param>
        /// <returns>360�� ���� �� ���� ���� ����</returns>
        public static float Angle360(Vector2 from, Vector2 to)
        {
            float angle = Vector2.SignedAngle(from, to);
            if (angle < 0) angle += 360;
            return angle;
        }

        /// <summary>
        /// Vector2 ���� vector�� delta(degree)��ŭ ȸ���� ���͸� ��ȯ
        /// </summary>
        /// <param name="vector">ȸ����ų ����</param>
        /// <param name="delta">ȸ���� ����(degree)</param>
        /// <returns>delta(degree)��ŭ ȸ���� vector</returns>
        public static Vector2 RotateVector(Vector2 vector, float delta)
        {
            float radian = Deg2Rad * delta;
            return new Vector2(vector.x * Cos(radian) - vector.y * Sin(radian), vector.x * Sin(radian) + vector.y * Cos(radian));
        }

        /// <summary>
        /// Vector2 ���� vector�� delta(degree)��ŭ ȸ���� ���͸� ��ȯ
        /// ��, ���밢��(x�� ���� ���� ����) clamp���� ���� ȸ���� ��� clamp������ ȸ���� ���͸� ��ȯ
        /// </summary>
        /// <param name="vector">ȸ���� ����</param>
        /// <param name="delta">ȸ���� ����(degree)</param>
        /// <param name="clamp">ȸ�� �Ѱ� ����(x�� ���� ���� ����)</param>
        /// <returns>delta��ŭ ȸ���� ���� (�ִ� clamp)</returns>
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

