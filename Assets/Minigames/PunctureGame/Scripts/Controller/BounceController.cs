using System;
using System.Collections;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    [RequireComponent(typeof(Player))]
    public class BounceController : MonoBehaviour
    {
        [SerializeField] private float bounceTime = 0.15f;
        [SerializeField] private AnimationCurve bounceCurve;

        private float origGravity = 0.0f;
        private float origGravityLerp = 0.0f;
        
        [SerializeField] [ReadOnly] private bool isBouncing = false;

        [SerializeField] private Player player;
        private EntityController motion;

        private Coroutine bounceRoutine = null;

        public bool IsBouncing
        {
            get => isBouncing;
            private set => isBouncing = value;
        }

        private void Awake()
        {
            motion = player.motion;
        }

        public void BounceJump(Collider2D col)
        {
            if (IsBouncing) return;
            if (bounceRoutine != null)
            {
                player.SetAnimationState(Player.AnimationState.ExitBounce);
                StopCoroutine(bounceRoutine);
            }
            bounceRoutine = StartCoroutine(BounceJumpCoroutine(col));
        }

        private IEnumerator BounceJumpCoroutine(Collider2D col)
        {
            IsBouncing = true;
            col.enabled = false;
            player.SetAnimationState(Player.AnimationState.EnterBounce);
            
            print(col.name + "/" + col.transform.position.x + "/" + player.currentPos.x + "/" + (motion.isFacingLeft ? "LEFT" : "RIGHT"));
            
            if ((col.transform.position.x <= player.currentPos.x && motion.isFacingLeft) || // Player bounced enemy's right side
                (col.transform.position.x > player.currentPos.x && !motion.isFacingLeft))   // Player bounced enemy's left side
            {
                player.sprite.flipX = !player.sprite.flipX;
                motion.FlipDirection();
            }
            
            motion.SetActiveGroundCheck(false);

            var endTime = Time.time + bounceTime;

            ChangeBounceGravity();

            while (Time.time <= endTime)
            {
                var progressTime = endTime - Time.time;
                var curveScale = bounceCurve.Evaluate(progressTime / bounceTime);
                motion.gravity = origGravity * curveScale;
                yield return null;
            }

            RestoreGravity();
            
            motion.SetActiveGroundCheck(true);
            
            player.SetAnimationState(Player.AnimationState.ExitBounce);
            col.enabled = true;
            IsBouncing = false;
        }

        private void ChangeBounceGravity()
        {
            origGravity = motion.gravity;
            origGravityLerp = motion.gravityLerpInterval;
            motion.gravityLerpInterval = 1;
        }

        private void RestoreGravity()
        {
            motion.gravity = origGravity;
            motion.gravityLerpInterval = origGravityLerp;
        }
    }
}