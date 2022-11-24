using System;
using System.Collections;
using Redcode.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameHeaven.PunctureGame
{
    public class Enemy : MonoBehaviour, IEntityLogic, IPoolObject
    {
        [SerializeField] private string type;

        // Variables
        [SerializeField] private Controller2D controller;
        [SerializeField] private GameObject colsParentObj;

        [SerializeField] private Animator animator;
        private static readonly int TreadAnimParam = Animator.StringToHash("Tread");

        public event Action OnTrodden;

        private Coroutine stunCoroutine;
        
        public string Type => type;

        public Controller2D Controller => controller;

        private void Update()
        {
            controller.Move();
        }

        private void FixedUpdate()
        {
            controller.GroundCheck();
        }

        private void OnDisable()
        {
            if (stunCoroutine != null)
            {
                StopCoroutine(stunCoroutine);
                SetStunState(false);
            }
        }

        public void Stun(float time)
        {
            stunCoroutine = StartCoroutine(StunCoroutine(time));
        }

        private IEnumerator StunCoroutine(float time)
        {
            if (!controller.CanControl) yield break;
            
            SetStunState(true);
            OnTrodden?.Invoke();
            yield return new WaitForSeconds(time);
            SetStunState(false);
        }

        private void SetStunState(bool value)
        {
            controller.CanControl = !value;
            animator.SetBool(TreadAnimParam, value);
            colsParentObj.SetActive(!value);
        }

        public void SetPositionState(Vector3 point)
        {
            transform.position = point;

            var isRight = Random.Range(0, 2) == 0;
            controller.SetStartSide(isRight);
            var localScale = colsParentObj.transform.localScale;
            localScale.Set(-localScale.x, localScale.y, localScale.z);
        }

        public void Ready()
        {
            controller.CanControl = false;
            animator.speed = 0.0f;
        }

        public void Active()
        {
            controller.CanControl = true;
            animator.speed = 1.0f;
            colsParentObj.SetActive(true);
        }

        public void Inactive()
        {
            if (stunCoroutine != null)
            {
                StopCoroutine(stunCoroutine);
            }
            controller.CanControl = false;
            animator.speed = 0.0f;
            colsParentObj.SetActive(false);
        }

        public void OnCreatedInPool()
        {
            Inactive();
        }

        public void OnGettingFromPool()
        {
            Active();
        }
    }
}