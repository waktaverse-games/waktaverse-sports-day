using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame {
    /*public enum EnemyType {
        Panchi,
        Gorani,
        Segyun,
        Dulgi,
        PoopPuppy,
        Bat,
        Jupokdo
    }*/

    public class EnemyController : MonoBehaviour {
        public delegate void PoolReleaseEvent(EnemyController enemyController);

        private static readonly int layerCount = 2;

        // Variables
        [Header("Enemy Objects")]
        [SerializeField] private List<GameObject> enemyObjs;

        [Header("Control Value")]
        [SerializeField] private float releaseHeight;
        [SerializeField] private float earlyReleaseHeight;
        [SerializeField] private float adjustWidth;

        [Header("Components")]
        [ReadOnly] public PlayerController playerController;
        public XAxisMovement movement;
        [SerializeField] private SpriteRenderer spRender;
        [SerializeField] private Animator anim;
        
        public PoolReleaseEvent onRelease;

        private void Update() {
            CheckRelease();
            AdjustXAxis();
        }

        private void OnEnable() {
            playerController = FindObjectOfType<PlayerController>();
        }

        private void AdjustXAxis() {
            var position = transform.position;

            var dist = position.x - playerController.currentPos.x;
            if (dist < -adjustWidth)
                position = new Vector3(playerController.currentPos.x + adjustWidth,
                    position.y, position.z);
            else if (dist > adjustWidth)
                position = new Vector3(playerController.currentPos.x - adjustWidth,
                    position.y, position.z);

            transform.position = position;
        }

        private void CheckRelease() {
            var enemyPos = transform.position;
            var playerPos = playerController.currentPos;
            if (enemyPos.y - playerPos.y > releaseHeight) onRelease.Invoke(this);
            if (enemyPos.y - playerPos.y > earlyReleaseHeight) {
                var xDist = enemyPos.x - playerPos.x;
                if ((xDist < -adjustWidth) || (xDist > adjustWidth)) {
                    onRelease.Invoke(this);
                }
            }
        }

        // Initialize Methods

        public void Spawn(Vector3 point) {
            foreach (var obj in enemyObjs) {
                obj.SetActive(false);
            }

            var enemyObj = enemyObjs[Random.Range(0, enemyObjs.Count)];
            enemyObj.SetActive(true);

            spRender = enemyObj.GetComponent<SpriteRenderer>();
            anim = enemyObj.GetComponent<Animator>();
            
            transform.position = point;
            var isLeft = Random.Range(0, 2) == 1;
            movement.isFacingLeft = isLeft;
            spRender.flipX = !isLeft;
        }
    }
}