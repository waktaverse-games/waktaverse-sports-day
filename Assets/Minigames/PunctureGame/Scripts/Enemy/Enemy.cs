using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    public class Enemy : MonoBehaviour, IEntityLogic
    {
        public delegate void PoolReleaseEvent(Enemy enemy);

        // Variables
        [SerializeField] private List<GameObject> skins;

        [SerializeField] private float releaseHeight;

        [SerializeField] private float earlyReleaseHeight;
        [SerializeField] private float adjustWidth;

        [SerializeField] [ReadOnly] private Player player;

        [SerializeField] private EntityController motion;
        
        [SerializeField] private SpriteRenderer enemySprite;
        [SerializeField] private Animator animator;
        [SerializeField] private Collider2D[] cols;

        public PoolReleaseEvent OnRelease;

        private void OnEnable()
        {
            player = FindObjectOfType<Player>();
        }

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

            SetStunState(true);
            yield return new WaitForSeconds(3.0f);
            SetStunState(false);
        }

        private void SetStunState(bool value)
        {
            motion.SetMovable(!value);
            foreach (var col in cols)
            {
                col.enabled = !value;
            }
        }

        // Initialize Methods

        public Enemy Spawn()
        {
            var skinComp = SelectRandomSkin();
            enemySprite = skinComp.Item1;
            animator = skinComp.Item2;

            gameObject.SetActive(true);
            return this;
        }

        public void Init(Vector3 point)
        {
            transform.position = point;
            var isLeft = Random.Range(0, 2) == 1;
            motion.isFacingLeft = isLeft;
            enemySprite.flipX = !isLeft;
        }

        private (SpriteRenderer, Animator) SelectRandomSkin()
        {
            foreach (var obj in skins)
            {
                obj.SetActive(false);
            }
            var skinObj = skins[Random.Range(0, skins.Count)];
            skinObj.SetActive(true);

            var render = skinObj.GetComponent<SpriteRenderer>();
            var anim = skinObj.GetComponent<Animator>();
            return (render, anim);
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