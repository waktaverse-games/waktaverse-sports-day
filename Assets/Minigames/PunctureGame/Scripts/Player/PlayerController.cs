using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class PlayerController : MonoBehaviour
    {
        // Variables
        [SerializeField] private LayerMask blockLayer;

        [SerializeField] private bool isFacingLeft = true;
        [SerializeField] private float moveSpeed = 2.0f;
        
        [SerializeField] private float gravity = 4.0f;
        [SerializeField, ReadOnly] private float lerpGravity = 0.0f;
        [SerializeField] private float itpVal = 0.05f;
        
        [SerializeField] private float groundCheckRad = 0.06f;
        [SerializeField] private Vector2 blockCheckSize = Vector2.one * 0.2f;
        [SerializeField, ReadOnly] private bool isGrounded = false;

        [SerializeField, ReadOnly] private Transform[] hitBlocks = null;
        
        // Properties
        public bool IsFacingLeft => isFacingLeft;
        public Vector3 currentPos => transform.position;
        
        private Vector3 MoveDir => isFacingLeft ? Vector3.left : Vector3.right;
        private Vector3 GravityVec
        {
            get
            {
                if (isGrounded)
                {
                    lerpGravity = 0.0f;
                    return Vector3.zero;
                }
                lerpGravity = Mathf.Lerp(lerpGravity, gravity, itpVal);
                return lerpGravity * Vector3.down;
            }
        }
        private Vector3 MoveVec => (MoveDir * moveSpeed + GravityVec) * Time.deltaTime;
        
        // Unity Events
        private void Awake()
        {
            
        }

        private void Start()
        {

        }

        private void Update()
        {
            // Break Block
            if (Input.GetKeyDown(KeyCode.F))
            {
                hitBlocks = DetectBlocks();
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
                isFacingLeft = !isFacingLeft;
            }
            
            Move();
            FloorDetect();
        }

        private void Move()
        {
            transform.position += MoveVec;
        }

        private void FloorDetect()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down * groundCheckRad, 0.0f, blockLayer);
            if (hit && !isGrounded)
            {
                var position = transform.position;
                position = new Vector3(position.x, Mathf.RoundToInt(position.y), position.z);
                transform.position = position;
            }
            isGrounded = hit;
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
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, groundCheckRad);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, blockCheckSize);
        }
    }
}