using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class PlayerController : TwoAxisEntity
    {
        // Variables
        [SerializeField] private Vector2 blockCheckSize = Vector2.one * 0.2f;
        
        // Properties
        public Vector3 currentPos => transform.position;
        
        // Unity Events
        private void Update()
        {
            // Break Block
            if (Input.GetKeyDown(KeyCode.F))
            {
                var hitBlocks = DetectBlocks();
                if (hitBlocks != null)
                {
                    foreach (var block in hitBlocks)
                    {
                        block.gameObject.SetActive(false);
                    }
                }
            }
            // (DEBUG) Change Direction
            if (Input.GetKeyDown(KeyCode.B))
            {
                ChangeFacing();
            }
            
            Move();
            FloorDetect();
        }

        private Transform[] DetectBlocks()
        {
            var hits = Physics2D.BoxCastAll(transform.position, blockCheckSize, 0.0f, Vector2.zero, 0.0f, blockLayer);
            if (hits.Length > 0)
            {
                var blocks = new Transform[hits.Length];
                for (var i = 0; i < hits.Length; i++)
                {
                    blocks[i] = hits[i].transform;
                }
                return blocks;
            }
            return null;
        }

        private void OnDrawGizmos()
        {
            GroundedGizmos();
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, blockCheckSize);
        }
    }
}