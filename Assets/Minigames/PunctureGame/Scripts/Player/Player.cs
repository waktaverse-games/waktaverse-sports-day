using System;
using GameHeaven.PunctureGame.Utils;
using SharedLibs.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class Player : LogicBehaviour
    {
        [SerializeField] private Vector2 blockCheckSize;
        [SerializeField] private Vector2 blockCheckPos;

        [SerializeField] private LayerMask blockLayer;
        [SerializeField] private LayerMask enemyHeadLayer;
        [SerializeField] private LayerMask enemyBodyLayer;

        [SerializeField] private PlayerController controller;
        [SerializeField] private PlayerObjectSwitch playerSwitch;

        [SerializeField] private KeyCode blockBreakKey;
        [SerializeField] [ReadOnly] private bool allowCollisionDetect;
        
        private RaycastHit2D[] blockHits;

        [SerializeField] private float enemyStunTime;

        [SerializeField] private float speedUpPerMin;
        [SerializeField] private float maxSpeed;

        [SerializeField] [ReadOnly] private Animator animator;
        private static readonly int RunAnimParam = Animator.StringToHash("Run");
        private static readonly int JumpAnimParam = Animator.StringToHash("Jump");
        private static readonly int BounceAnimParam = Animator.StringToHash("Bounce");
        private static readonly int GameOverAnimParam = Animator.StringToHash("GameOver");

        [SerializeField] private PunctureSFXCollection sfxCollection;
        
        public enum AnimationState
        {
            Idle,
            Run,
            Jump,
            EnterBounce,
            ExitBounce,
            GameOver
        }
        [SerializeField] [ReadOnly] private AnimationState currentAnimState = AnimationState.Idle;

        [SerializeField] private ParticleSpawner particleSpawner;

        private void Awake()
        {
            var charType = CharacterManager.Instance.CurrentCharacter;
            var obj = playerSwitch.Enable(charType);

            controller.TargetSprite = obj.GetComponent<SpriteRenderer>();
            animator = obj.GetComponent<Animator>();
            
            blockHits = new RaycastHit2D[10];
        }

        private void Update()
        {
            controller.Move();

            if (allowCollisionDetect)
            {
                if (Input.GetKeyDown(blockBreakKey)) BreakBlocks();
            }

            controller.Speed = SpeedUpPerMinute(controller.Speed, speedUpPerMin);
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
            if (!allowCollisionDetect) return;
            
            if (ColliderUtil.CheckColliderHitLayer(col, enemyBodyLayer))
            {
                GameManager.Instance.GameOver();
            }
            else if (ColliderUtil.CheckColliderHitLayer(col, enemyHeadLayer))
            {
                var enemy = col.GetComponentInParent<Enemy>();
                TreadEnemy(enemy, Vector3.Lerp(col.transform.position, controller.Position, 0.5f));
                
                controller.Bounce(col.transform);
            }
        }

        private void TreadEnemy(Enemy enemy, Vector3 treadPos)
        {
            sfxCollection.PlaySFX(PunctureSFXType.Bounce);
            particleSpawner.PlayParticle(ParticleType.Tread, treadPos);
            
            enemy.Stun(enemyStunTime);
        }

        private void BreakBlocks()
        {
            var playerPos = controller.Position;
            var checkPos = new Vector2(playerPos.x + (controller.IsRightSide ? -blockCheckPos.x : blockCheckPos.x),
                playerPos.y + blockCheckPos.y);
            var num = Physics2D.BoxCastNonAlloc(checkPos, blockCheckSize, 0.0f, Vector2.zero, blockHits, 0.0f,
                blockLayer);
            if (num > 0)
            {
                sfxCollection.PlaySFX(PunctureSFXType.BlockBreak);
                particleSpawner.PlayParticle(ParticleType.Break, controller.Position);
                
                for (var i = 0; i < num; i++) blockHits[i].transform.gameObject.SetActive(false);
            }
        }

        // ips: increase per second
        private float SpeedUpPerMinute(float speed, float up)
        {
            if (controller.CanControl)
            {
                speed += up * Time.deltaTime / 60f;
            }

            return Mathf.Clamp(speed, 0.0f, maxSpeed);
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
            allowCollisionDetect = false;
            
            controller.CanControl = false;
            animator.speed = 0.0f;
        }

        public override void GameStart()
        {
            allowCollisionDetect = true;
            
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