using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;
using Task = System.Threading.Tasks.Task;

namespace GameHeaven.PunctureGame
{
    public class Player : LogicAffectEntity
    {
        // Variables
        
        [SerializeField] private Vector2 blockCheckSize = Vector2.one * 0.2f;

        [SerializeField] private LayerMask blockLayer;
        [SerializeField] private LayerMask enemyHeadLayer;
        [SerializeField] private LayerMask enemyBodyLayer;

        public EntityController motion;
        [SerializeField] private BounceController bounce;

        public SpriteRenderer sprite;
        
        [SerializeField] private Animator animator;
        private static readonly int RunAnimParam = Animator.StringToHash("Run");
        private static readonly int JumpAnimParam = Animator.StringToHash("Jump");
        private static readonly int BounceAnimParam = Animator.StringToHash("Bounce");
        private static readonly int GameOverAnimParam = Animator.StringToHash("GameOver");
        
        public enum AnimationState
        {
            Idle, Run, Jump, EnterBounce, ExitBounce, GameOver
        }
        [SerializeField] [ReadOnly] private AnimationState currentAnimState = AnimationState.Idle;

        [SerializeField] private PlayerSFXCollection sfxCollect;

        // Properties
        
        public Vector3 currentPos => transform.position;

        // Unity Events

        private void Start()
        {
            Active();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                BreakBlocks();
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((enemyHeadLayer & (1 << col.gameObject.layer)) != 0)
            {
                bounce.BounceJump(col);
                var enemy = col.GetComponentInParent<Enemy>();
                TreadEnemy(enemy);
            }
            else if ((enemyBodyLayer & (1 << col.gameObject.layer)) != 0)
            {
                SetAnimationState(AnimationState.GameOver);
                GameManager.Instance.GameOver();
            }
        }
        
        // Bounce

        private void TreadEnemy(Enemy enemy)
        {
            sfxCollect.PlaySFX(PlayerSoundType.Bounce);
            enemy.Stun();
        }
        
        // Break Blocks

        private void BreakBlocks()
        {
            var hitBlocks = DetectBlocks();
            if (hitBlocks != null)
            {
                foreach (var block in hitBlocks)
                {
                    block.gameObject.SetActive(false);
                }
                sfxCollect.PlaySFX(PlayerSoundType.BlockBreak);
                SetAnimationState(AnimationState.Jump);
            }
        }

        private Transform[] DetectBlocks()
        {
            var hits = Physics2D.BoxCastAll(transform.position, blockCheckSize, 0.0f, Vector2.zero, 0.0f, blockLayer);
            if (hits.Length > 0)
            {
                var blocks = new Transform[hits.Length];
                for (var i = 0; i < hits.Length; i++) blocks[i] = hits[i].transform;
                return blocks;
            }

            return null;
        }
        
        // Animation

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
        
        // Interface Implement

        public override void Active()
        {
            SetAnimationState(AnimationState.Run);
            motion.SetMovable(true);
            sfxCollect.enabled = true;
            animator.speed = 1.0f;
        }

        public override void Inactive()
        {
            motion.SetMovable(false);
            sfxCollect.enabled = false;
            animator.speed = 0.0f;
        }

        // Gizmos
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, blockCheckSize);
        }
    }
}