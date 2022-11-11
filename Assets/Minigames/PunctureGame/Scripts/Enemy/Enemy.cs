using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SharedLibs;
using SharedLibs.Score;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    public class Enemy : MonoBehaviour, IEntityLogic
    {
        // Variables
        [SerializeField] private EntityController motion;
        
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Animator animator;
        private static readonly int TreadAnimParam = Animator.StringToHash("Tread");
        
        [SerializeField] private Collider2D[] cols;

        [SerializeField] private float treadTime = 1.0f;
        [SerializeField] private int treadScore = 10;

        private void OnDisable()
        {
            if (stunCoroutine != null)
            {
                StopCoroutine(stunCoroutine);
                SetStunState(false);
            }
        }

        private Coroutine stunCoroutine = null;

        public void Stun()
        {
            stunCoroutine = StartCoroutine(StunCoroutine());
        }

        private IEnumerator StunCoroutine()
        {
            if (!motion.IsMovable) yield break;
            
            ScoreManager.Instance.AddGameRoundScore(MinigameType.PunctureGame, treadScore);

            SetStunState(true);
            yield return new WaitForSeconds(treadTime);
            SetStunState(false);
        }

        private void SetStunState(bool value)
        {
            motion.SetMovable(!value);
            animator.SetBool(TreadAnimParam, value);
            foreach (var col in cols)
            {
                col.enabled = !value;
            }
        }

        // Initialize Methods

        public void SetPositionState(Vector3 point)
        {
            transform.position = point;
            
            var isLeft = Random.Range(0, 2) == 1;
            motion.isFacingLeft = isLeft;
            sprite.flipX = !isLeft;
        }
        
        // Interface Implement

        public void Active()
        {
            motion.SetMovable(true);
            animator.speed = 1.0f;
        }

        public void Inactive()
        {
            motion.SetMovable(false);
            animator.speed = 0.0f;
        }
    }
}