using System;
using GameHeaven.PunctureGame.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class Player : LogicBehaviour
    {
        public enum AnimationState
        {
            Idle,
            Run,
            Jump,
            EnterBounce,
            ExitBounce,
            GameOver
        }

        private static readonly int RunAnimParam = Animator.StringToHash("Run");
        private static readonly int JumpAnimParam = Animator.StringToHash("Jump");
        private static readonly int BounceAnimParam = Animator.StringToHash("Bounce");
        private static readonly int GameOverAnimParam = Animator.StringToHash("GameOver");

        [SerializeField] private Vector2 blockCheckSize;

        [SerializeField] private LayerMask blockLayer;
        [SerializeField] private LayerMask enemyHeadLayer;
        [SerializeField] private LayerMask enemyBodyLayer;

        [SerializeField] private PlayerController controller;

        [SerializeField] private KeyCode blockBreakKey;
        [SerializeField] [ReadOnly] private bool allowBreakBlock;

        [SerializeField] private float enemyStunTime;

        [SerializeField] private Animator animator;
        [SerializeField] [ReadOnly] private AnimationState currentAnimState = AnimationState.Idle;

        [SerializeField] private ParticleSystem treadParticle, breakParticle;
        
        private RaycastHit2D[] blockHits;

        private void Awake()
        {
            blockHits = new RaycastHit2D[10];
        }

        private void Update()
        {
            controller.Move();

            if (allowBreakBlock)
            {
                if (Input.GetKeyDown(blockBreakKey)) BreakBlocks();
            }
        }

        private void FixedUpdate()
        {
            controller.GroundCheck();
        }

        private void OnEnable()
        {
            controller.OnFalling += () => SetAnimationState(AnimationState.Jump);
            controller.OnGrounded += () => SetAnimationState(AnimationState.Run);

            controller.OnStartBounce += () => SetAnimationState(AnimationState.EnterBounce);
            controller.OnEndBounce += () => SetAnimationState(AnimationState.ExitBounce);
            controller.OnStopBounce += () => SetAnimationState(AnimationState.ExitBounce);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (ColliderUtil.CheckColliderHitLayer(col, enemyBodyLayer))
            {
                GameManager.Instance.GameOver();
            }
            else if (ColliderUtil.CheckColliderHitLayer(col, enemyHeadLayer))
            {
                controller.Bounce(col.transform);
                var enemy = col.GetComponentInParent<Enemy>();
                TreadEnemy(enemy);
            }
        }

        private void TreadEnemy(Enemy enemy)
        {
            PunctureSFXCollection.Instance.PlaySFX(PunctureSFXType.Bounce);
            treadParticle.Play();
            
            enemy.Stun(enemyStunTime);
        }

        private void BreakBlocks()
        {
            var num = DetectBlocks();
            if (num > 0)
            {
                PunctureSFXCollection.Instance.PlaySFX(PunctureSFXType.BlockBreak);
                breakParticle.Play();
                
                for (var i = 0; i < num; i++) blockHits[i].transform.gameObject.SetActive(false);
            }
        }

        private int DetectBlocks()
        {
            return Physics2D.BoxCastNonAlloc(transform.position, blockCheckSize, 0.0f, Vector2.zero, blockHits, 0.0f,
                blockLayer);
        }

        public void SetAnimationState(AnimationState state)
        {
            switch (state)
            {
                case AnimationState.Idle:
                case AnimationState.Run:
                    animator.SetBool(RunAnimParam, true);
                    break;
                case AnimationState.Jump:
                    if (currentAnimState == AnimationState.Run) animator.SetTrigger(JumpAnimParam);
                    state = AnimationState.Run;
                    break;
                case AnimationState.EnterBounce:
                    if (currentAnimState == AnimationState.Run) animator.SetBool(BounceAnimParam, true);
                    break;
                case AnimationState.ExitBounce:
                    if (currentAnimState == AnimationState.EnterBounce) animator.SetBool(BounceAnimParam, false);
                    state = AnimationState.Run;
                    break;
                case AnimationState.GameOver:
                    animator.SetBool(GameOverAnimParam, true);
                    break;
            }

            currentAnimState = state;
        }

        public override void GameReady()
        {
            allowBreakBlock = false;
            
            controller.CanControl = false;
            Debug.Log("플레리러 레듸" + controller.CanControl);
            animator.speed = 0.0f;
        }

        public override void GameStart()
        {
            allowBreakBlock = true;
            
            SetAnimationState(AnimationState.Run);
            controller.CanControl = true;
            animator.speed = 1.0f;
        }

        public override void GameOver()
        {
            SetAnimationState(AnimationState.GameOver);
            controller.CanControl = false;
            animator.speed = 0.0f;
        }
    }
}