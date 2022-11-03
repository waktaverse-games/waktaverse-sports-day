using System;
using System.Collections;
using UnityEngine;

namespace GameHeaven.PunctureGame {
    public class PlayerController : MonoBehaviour {
        // Variables
        [Header("Block Checking")]
        [SerializeField] private Vector2 blockCheckSize = Vector2.one * 0.2f;
        [SerializeField] private LayerMask blockLayer;

        [Header("Components")]
        public XAxisMovement movement;
        [SerializeField] private SpriteRenderer spRender;
        [SerializeField] private Animator anim;
        [SerializeField] private Collider2D hitCol;
        
        private Coroutine jumpCoroutine;

        // Properties
        public Vector3 currentPos => transform.position;

        // Unity Events
        private void Update() {
            // Break Block
            if (Input.GetKeyDown(KeyCode.F)) {
                var hitBlocks = DetectBlocks();
                if (hitBlocks != null)
                    foreach (var block in hitBlocks)
                        block.gameObject.SetActive(false);
            }

            // (DEBUG) Change Direction
            if (Input.GetKeyDown(KeyCode.B)) movement.ChangeDirection();
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.layer == 7) {
                Debug.Log("Gravity Reverse");
                if (jumpCoroutine != null) {
                    StopCoroutine(jumpCoroutine);
                }
                jumpCoroutine = StartCoroutine(nameof(Jump));
            }
            else if (col.gameObject.layer == 8) {
                Debug.Log("Game Over");
                Debug.Break();
            }
        }

        private IEnumerator Jump() {
            hitCol.enabled = false;
            
            var origGrav = movement.gravity;
            var origIntv = movement.gravityLerpInterval;
            var origSpeed = movement.moveSpeed;
            
            movement.gravity = -1.25f;
            movement.gravityLerpInterval = 0.75f;
            movement.moveSpeed = origSpeed/2f;
            
            movement.ChangeDirection();
            spRender.flipX = !spRender.flipX;
            
            yield return new WaitForSeconds(0.35f);
            
            movement.gravity = origGrav;
            movement.gravityLerpInterval = 0.036f;
            
            yield return new WaitForSeconds(0.25f);
            
            movement.gravity = origGrav;
            movement.gravityLerpInterval = origIntv;
            movement.moveSpeed = origSpeed;
            
            hitCol.enabled = true;
        }

        private Transform[] DetectBlocks() {
            var hits = Physics2D.BoxCastAll(transform.position, blockCheckSize, 0.0f, Vector2.zero, 0.0f, blockLayer);
            if (hits.Length > 0) {
                var blocks = new Transform[hits.Length];
                for (var i = 0; i < hits.Length; i++) blocks[i] = hits[i].transform;
                return blocks;
            }

            return null;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, blockCheckSize);
        }
    }
}