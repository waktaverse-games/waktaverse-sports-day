using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

// Recommand to Player start left direction; 
namespace GameHeaven.PunctureGame
{
    public class PlayerController : Controller2D
    {
        [SerializeField] private float bounceTime = 0.15f;
        [SerializeField] private AnimationCurve bounceCurve;

        [SerializeField] [ReadOnly] private bool isBouncing;

        private Coroutine bounceRoutine;

        private float originGravity;
        private float originGravityIntrp;

        public event Action OnStartBounce;
        public event Action OnEndBounce;
        public event Action OnStopBounce;

        public void Bounce(Transform enemy)
        {
            if (isBouncing) return;
            if (bounceRoutine != null) StopCoroutine(bounceRoutine);
            bounceRoutine = StartCoroutine(BounceJumpCoroutine(enemy));
        }

        private IEnumerator BounceJumpCoroutine(Transform enemy)
        {
            isBouncing = true;
            CanGroundCheck = false;

            OnStartBounce?.Invoke();

            if ((enemy.position.x <= transform.position.x && !IsRightSide) || // Player bounced enemy's right side
                (enemy.position.x > transform.position.x && IsRightSide)) // Player bounced enemy's left side
            {
                ReverseDirection();
            }

            SaveOriginGravityValues();

            gravityChangeIntrp = 1.0f;

            var endTime = Time.time + bounceTime;

            while (Time.time <= endTime)
            {
                var progressTime = endTime - Time.time;
                var curveScale = bounceCurve.Evaluate(progressTime / bounceTime);
                gravityPower = originGravity * curveScale;
                yield return null;
            }

            RestoreOriginGravityValues();

            OnEndBounce?.Invoke();

            CanGroundCheck = true;
            isBouncing = false;
        }

        private void SaveOriginGravityValues()
        {
            originGravity = gravityPower;
            originGravityIntrp = gravityChangeIntrp;
        }

        private void RestoreOriginGravityValues()
        {
            gravityPower = originGravity;
            gravityChangeIntrp = originGravityIntrp;
        }
    }
}