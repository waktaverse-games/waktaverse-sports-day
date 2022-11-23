using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class Controller2D : MonoBehaviour
    {
        [SerializeField] private LayerMask floorLayer;

        [SerializeField] private bool startRightSide;
        [SerializeField] [ReadOnly] private bool isReversed;
        public bool IsRightSide => startRightSide ^ isReversed;

        [SerializeField] private float speed;
        
        [SerializeField] protected float gravityPower;
        [SerializeField] protected float gravityChangeIntrp;
        [SerializeField] [ReadOnly] private float intrpGravityPower = 0.0f;
        
        private Vector3 Gravity
        {
            get
            {
                intrpGravityPower = isGrounded ? 0.0f : Mathf.Lerp(intrpGravityPower, gravityPower, gravityChangeIntrp);
                return intrpGravityPower * Vector3.down;
            }
        }

        private Vector3 Direction => IsRightSide ? Vector3.right : Vector3.left;
        public Vector3 MoveVector => (Direction * speed + Gravity);

        [SerializeField] private float groundCheckRad;
        
        [SerializeField] [ReadOnly] private bool isGrounded;
        
        [SerializeField] [ReadOnly] private bool canControl;
        public bool CanControl
        {
            get => canControl;
            set => canControl = value;
        }
        [SerializeField] [ReadOnly] private bool canGroundCheck;
        public bool CanGroundCheck
        {
            get => canGroundCheck;
            set => canGroundCheck = value;
        }

        public event Action OnFalling;
        public event Action OnGrounded;

        public Vector3 Position => transform.position;

        public void Move()
        {
            if (CanControl)
            {
                transform.position += MoveVector * Time.deltaTime;
            }
        }

        public void GroundCheck()
        {
            if (CanGroundCheck)
            {
                var hit = Physics2D.Raycast(transform.position, Vector2.down * groundCheckRad, 0.0f, floorLayer);
                if (hit && !isGrounded)
                {
                    transform.position = RoundYPos(transform.position);
                    OnGrounded?.Invoke();
                }
                else if (!hit && isGrounded)
                {
                    OnFalling?.Invoke();
                }

                isGrounded = hit;
            }
            else
            {
                isGrounded = false;
            }
        }

        private Vector3 RoundYPos(Vector3 pos)
        {
            pos.y = Mathf.RoundToInt(pos.y);
            return pos;
        }

        public bool ReverseDirection()
        {
            isReversed = !isReversed;
            return isReversed;
        }

        public void SetStartSide(bool isRight)
        {
            startRightSide = isRight;
        }
    }
}