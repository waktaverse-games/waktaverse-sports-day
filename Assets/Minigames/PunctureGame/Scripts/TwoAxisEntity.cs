using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public abstract class TwoAxisEntity : MonoBehaviour
    {
        // Variables
        public LayerMask blockLayer;
        
        [ReadOnly] public bool isFacingLeft = true;
        public float moveSpeed = 2.0f;
        
        public float gravity = 4.0f;
        public float gravityLerpInterval = 0.05f;
        [SerializeField, ReadOnly] private float lerpGravity = 0.0f;
        
        public float groundCheckRad = 0.06f;
        [SerializeField, ReadOnly] private bool isGrounded = false;
        
        // Properties
        public bool IsGrounded
        {
            get => isGrounded;
            set => isGrounded = value;
        }
        private Vector3 GravityVec
        {
            get
            {
                if (IsGrounded)
                {
                    lerpGravity = 0.0f;
                    return Vector3.zero;
                }
                lerpGravity = Mathf.Lerp(lerpGravity, gravity, gravityLerpInterval);
                return lerpGravity * Vector3.down;
            }
        }
        
        private Vector3 MoveDir => isFacingLeft ? Vector3.left : Vector3.right;
        private Vector3 MoveVec => (MoveDir * moveSpeed + GravityVec) * Time.deltaTime;

        public void Move()
        {
            transform.position += MoveVec;
        }

        public virtual void ChangeFacing()
        {
            isFacingLeft = !isFacingLeft;
        }

        public void FloorDetect()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down * groundCheckRad, 0.0f, blockLayer);
            if (hit && !IsGrounded)
            {
                var position = transform.position;
                position = new Vector3(position.x, Mathf.RoundToInt(position.y), position.z);
                transform.position = position;
            }
            IsGrounded = hit;
        }

        public void GroundedGizmos()
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, groundCheckRad);
        }
    }
}